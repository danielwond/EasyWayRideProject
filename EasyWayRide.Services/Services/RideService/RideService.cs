using AutoMapper;
using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.DataAccess.Repos.IConfigRepo;
using EasyWayRide.DataAccess.Repos.IRideRepo;
using EasyWayRide.Services.Services.NotificationService;
using EasyWayRide.Services.Services.PassengerService;
using EasyWayRide.Shared.DTOs.Google;
using EasyWayRide.Shared.DTOs.Ride;
using EasyWayRide.Shared.Options;
using EasyWayRide.Shared.Responses;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EasyWayRide.Services.Services.RideService
{
    public class RideService : IRideService
    {
        private readonly IRideRepository _repository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IPassengerService _passengerService;
        private readonly IPushNotificationService _notificationService;
        private readonly IConfigRepository _configRepository;
        private readonly GoogleOptions _googleOptions;

        public RideService(IRideRepository repository,
            IMapper mapper,
            HttpClient httpClient,
            IOptions<GoogleOptions> googleOptions,
            IPassengerService passengerService,
            IPushNotificationService notificationService, IConfigRepository configRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _httpClient = httpClient;
            _passengerService = passengerService;
            _notificationService = notificationService;
            _configRepository = configRepository;
            _googleOptions = googleOptions.Value;
        }
        public async Task<ServiceResponseData<AcceptRideResponseDTO>> AcceptRide(Guid requestID, Guid driverID)
        {
            var result = await _repository.AcceptRide(requestID, driverID);
            return result;
        }

        public async Task<ServiceResponseMessage> CancelRide(Guid requestID, string? cancellationReason)
        {
            var result = await _repository.CancelRide(requestID, cancellationReason);
            return result;
        }

        public async Task<ServiceResponseData<EndRideResponseDTO>> EndRide(Guid requestID, string EndLocationName, string EndLocationCoordinates, decimal EndLatitude, decimal EndLongitude)
        {
            var req = await _repository.GetRideRequestData(requestID);

            var startLatitude = req.Data.Location.StartLatitude;
            var startLongitude = req.Data.Location.StartLongitude;

            var distance = await CalculateDistance(startLongitude, startLatitude, EndLongitude, EndLatitude);
            var result = await _repository.EndRide(requestID, EndLocationName, EndLocationCoordinates, EndLatitude, EndLongitude, distance);

            //TODO: Notify Passenger Service Ride has Ended

            return result;
        }

        public async Task<ServiceResponseMessage> StartRide(Guid requestID, string StartLocationName, string StartLocationCoordinates, decimal StartLatitude, decimal StartLongitude)
        {
            var result = await _repository.StartRide(requestID, StartLocationName, StartLocationCoordinates, StartLatitude, StartLongitude);

            //TODO: Notify Passenger Ride has started

            return result;
        }

        public async Task<ServiceResponseData<SubmitRideRequestResponseDTO>> SubmitRideRequest(CreateRideRequestDTO ride, Guid PassengerID, Guid CarTypeID)
        {

            var ride_request = _mapper.Map<RideRequest>(ride);
            var loc = new Location()
            {
                ID = new Guid(),
                RequestID = ride_request.ID,
                DestinationLatitude = ride.DestinationLatitude,
                DestinationCoordinates = ride.DestinationCoordinates,
                DestinationLongitude = ride.DestinationLongitude,
                DestinationName = ride.DestinationName,

                StartLatitude = ride.StartLatitude,
                StartLongitude = ride.StartLongitude,
                StartLocationCoordinates = ride.StartLocationCoordinates,
                StartLocationName = ride.StartLocationName,
            };

            ride_request.Location = loc;

            var req = await _repository.SubmitRideRequest(ride_request, PassengerID, CarTypeID);

            if (!req.Success)
            {
                return new ServiceResponseData<SubmitRideRequestResponseDTO>()
                {
                    Message = req.Message
                };
            }

            var result = await MakeRideRequestToDrivers(req.Data, CarTypeID);
            if (result == "TIME_OUT" || result == string.Empty)
            {
                return new ServiceResponseData<SubmitRideRequestResponseDTO>()
                {
                    Message = "TIMEOUT"
                };
            }
            else if (result == "no_drivers")
            {
                return new ServiceResponseData<SubmitRideRequestResponseDTO>()
                {
                    Message = "no_drivers"
                };
            }

            return req;
        }

        public async Task<double> CalculateDistance(decimal startLongitude, decimal startLatitude, decimal endLongitude, decimal endLatitude)
        {
            // The origin and destination latitude and longitude coordinates.
            string origin = $"{startLatitude}, {startLongitude}";
            string destination = $"{endLatitude},{endLongitude}";

            // The Google Distance Matrix API URL.
            string url = "https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + origin + "&destinations=" + destination + $"&key={_googleOptions.MapKey}";

            // Create a web request to the Google Distance Matrix API.
            var request = await _httpClient.GetAsync(url);

            // Get the response from the Google Distance Matrix API.
            var response = await request.Content.ReadAsStringAsync();

            DistanceMatrixResponse distanceMatrixResponse = JsonSerializer.Deserialize<DistanceMatrixResponse>(response);

            double distance = 0;
            var rows = distanceMatrixResponse.Rows.FirstOrDefault();
            var elements = rows.Elements.FirstOrDefault();

            if (distanceMatrixResponse != null && rows != null && elements != null)
            {
                distance = elements.Distance.Value;
            }
            return Convert.ToDouble(distance) / 1000;
        }


        private async Task<string> MakeRideRequestToDrivers(SubmitRideRequestResponseDTO ride, Guid CarTypeID)
        {
            DateTime startTime = DateTime.Now;
            var config = await _configRepository.GetAllConfigValues();
            var requestTimeout = config.Data!.Where(x => x.Name == Shared.Enums.ConfigNameEnumId.ride_waiting_time).FirstOrDefault();
            var driverTimeout = config.Data!.Where(x => x.Name == Shared.Enums.ConfigNameEnumId.driver_waiting_time).FirstOrDefault();

            //Get Driver FCM Tokens within that specific radius
            var tokens = await _passengerService.GetDriversWithinRadiusAsync((double)ride.StartLatitude, (double)ride.StartLongitude);

            if (tokens.Drivers == null)
            {
                return "no_drivers";
            }

            var driverFcmTokens = tokens.Drivers.Where(x => x.CarTypeID == CarTypeID.ToString()).Where(x => !x.onRide!.Value).Select(x => x.FcmToken).ToList();
            var driverFcmTokensProcessed = new List<string>();

            //foreach (var token in driverFcmTokens.Except(driverFcmTokensProcessed))
            foreach (var token in driverFcmTokens)
            {
                TimeSpan elapsedTime = DateTime.Now - startTime;
                if (elapsedTime.TotalSeconds >= (int)requestTimeout!.Value)
                {
                    return "TIME_OUT";
                }

                var req2 = await _repository.GetRideRequestData(ride.RequestID);
                // Check if the ride status is pending.
                if (req2.Data != null && req2.Data.RideStatus == Shared.Enums.RideStatusEnum.Pending)
                {
                    await _notificationService.SendFcmMessageAsync(token, $"RequestId: {req2.Data.ID}",
                        $"FROM: {req2.Data.Location.StartLocationName} , " +
                        $"TO:{req2.Data.Location.DestinationName} , " +
                        $"Passenger: {req2.Data.Passenger.FirstName} {req2.Data.Passenger.LastName} , " +
                        $"Passenger_Phone_Number: {req2.Data.Passenger.PhoneNumber}, " +
                        $"StartLocation: {req2.Data.Location.StartLatitude} # {req2.Data.Location.StartLongitude} , " +
                        $"DestinationLocation: {req2.Data.Location.DestinationLatitude} # {req2.Data.Location.DestinationLongitude}");
                    await Task.Delay((int)driverTimeout!.Value * 1000);

                    tokens = await _passengerService.GetDriversWithinRadiusAsync((double)ride.StartLatitude, (double)ride.StartLongitude);
                    var newDriverFcmTokens = tokens.Drivers.Select(x => x.FcmToken);

                    driverFcmTokens = driverFcmTokens.Union(newDriverFcmTokens).ToList();
                }
                else
                {
                    return "ride_accepted";
                }
                driverFcmTokensProcessed.Add(token);
            }

            return string.Empty;
        }
    }
}
