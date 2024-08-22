using EasyWayRide.DataAccess.Data;
using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.DataAccess.Repos.ICachingRepo;
using EasyWayRide.Shared.DTOs.Ride;
using EasyWayRide.Shared.Enums;
using EasyWayRide.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace EasyWayRide.DataAccess.Repos.IRideRepo
{
    public class RideRepository : IRideRepository
    {
        private readonly EasyWayRideDataContext _context;
        private readonly ICachingRepository _cachingRepository;
        private readonly IDatabase _database;

        public RideRepository(EasyWayRideDataContext context, ICachingRepository cachingRepository)
        {
            _context = context;
            _cachingRepository = cachingRepository;
            _database = _cachingRepository.InitializeRedis().GetDatabase();
        }
        public async Task<ServiceResponseData<AcceptRideResponseDTO>> AcceptRide(Guid requestID, Guid driverID)
        {
            //validate request
            var req = await _context.RideRequests.FirstOrDefaultAsync(x => x.ID == requestID);
            if (req == null)
            {
                return new ServiceResponseData<AcceptRideResponseDTO>()
                {
                    Message = "invalid_request_id"
                };
            }

            //validate ride status
            if (req.RideStatus != RideStatusEnum.Pending)
            {
                return new ServiceResponseData<AcceptRideResponseDTO>()
                {
                    Message = "cannot_accept_a_non_pending_ride"
                };
            };

            //validate driver
            var driver = await _context.Drivers.FirstOrDefaultAsync(x => x.ID == driverID);
            if (driver == null)
            {
                return new ServiceResponseData<AcceptRideResponseDTO>()
                {
                    Message = "invalid_driver_id"
                };
            }

            //change driver status if online
            var driverId = driver.ID;
            string? onRideString = await _database.HashGetAsync($"online-drivers:{driverId}", "OnRide");
            bool onRide = onRideString != "0";

            var online_driver = await _database.KeyExistsAsync($"online-drivers:{driverId}");

            if (onRide || !online_driver)
            {
                return new ServiceResponseData<AcceptRideResponseDTO>()
                {
                    Message = "driver_on_ride_or_invalid"
                };
            }

            //TODO: UNCOMMENT ON PRODUCTION
            //await _database.HashSetAsync($"online-drivers:{driverId}", "OnRide", true);

            req.RideStatus = RideStatusEnum.Accepted;
            req.Driver = driver;
            req.AcceptedOnDate = DateTime.UtcNow;

            _context.RideRequests.Update(req);
            await _context.SaveChangesAsync();

            var req_info = new AcceptRideResponseDTO()
            {
                Driver = new DriverAcceptResponseDTO()
                {
                    Gender = driver.Gender,
                    Name = driver.FirstName + " " + driver.LastName,
                    PhoneNumber = driver.PhoneNumber,
                    DriverID = driver.ID
                }
            };

            return new ServiceResponseData<AcceptRideResponseDTO>()
            {
                Data = req_info,
                Message = "ride_accepted",
                Success = true
            };
        }


        public async Task<ServiceResponseMessage> CancelRide(Guid requestID, string? cancellationReason)
        {
            var result = await _context.RideRequests.FirstOrDefaultAsync(x => x.ID == requestID);
            if (result == null)
            {
                return new ServiceResponseMessage()
                {
                    Message = "invalid_id",
                };
            }

            if (result.RideStatus != RideStatusEnum.Pending)
            {
                return new ServiceResponseMessage()
                {
                    Message = "cannot-cancel-a-not-pending-ride"
                };
            }
            result.CancellationReason = cancellationReason;
            result.RideStatus = RideStatusEnum.Cancelled;
            result.CancelledOnDate = DateTime.UtcNow;

            _context.RideRequests.Update(result);

            await _context.SaveChangesAsync();
            return new ServiceResponseMessage()
            {
                Message = "ride_started",
                Success = true
            };
        }


        public async Task<ServiceResponseData<EndRideResponseDTO>> EndRide(Guid requestID, string EndLocationName, string EndLocationCoordinates, decimal EndLatitude, decimal EndLongitude, double distance)
        {
            var result = await _context.RideRequests.Include(x => x.Driver).Include(x => x.CarType).Include(x => x.Location).FirstOrDefaultAsync(x => x.ID == requestID);
            if (result == null)
            {
                return new ServiceResponseData<EndRideResponseDTO>()
                {
                    Message = "invalid_id",
                };
            }
            if (result.RideStatus != RideStatusEnum.OnRide)
            {
                return new ServiceResponseData<EndRideResponseDTO>()
                {
                    Message = "cannot_end_a_not_ongoing_ride",
                };
            };

            var driverId = result.Driver.ID;
            string? onRideString = await _database.HashGetAsync($"online-drivers:{driverId}", "OnRide");
            bool onRide = bool.Parse(onRideString);

            var online_driver = await _database.KeyExistsAsync($"online-drivers:{driverId}");

            if (!onRide || !online_driver)
            {
                return new ServiceResponseData<EndRideResponseDTO>()
                {
                    Message = "invalid_data_driver"
                };
            }

            result.RideStatus = RideStatusEnum.Completed;
            result.EndedOnDate = DateTime.Now;

            result.Location!.EndLatitude = EndLatitude;
            result.Location.EndLatitude = EndLongitude;
            result.Location.EndLocationName = EndLocationName;
            result.Location.EndLocationCoordinates = EndLocationCoordinates;

            //change driver status if online
            //TODO: UNCOMMENT ON PRODUCTION
            //await _database.HashSetAsync($"online-drivers:{driverId}", "OnRide", false);

            decimal carBasePrice = result.CarType.BasePrice;
            decimal distanceRate = result.CarType.PerKmRate;
            decimal timeRate = result.CarType.PerMinuteRate;
            double totalRideTime = (result.EndedOnDate - result.StartedOnDate).Value.TotalMinutes;

            var totalFare = CalculateTotalFare(carBasePrice, distanceRate, timeRate, distance, totalRideTime);

            result.TotalFare = totalFare;

            _context.RideRequests.Update(result);
            await _context.SaveChangesAsync();

            return new ServiceResponseData<EndRideResponseDTO>()
            {
                Message = "ride_ended",
                Success = true,
                Data = new EndRideResponseDTO()
                {
                    From = result.Location.StartLocationName,
                    To = result.Location.EndLocationName,
                    TotalFare = totalFare,
                    TotalMinutes = totalRideTime
                }
            };
        }

        public async Task<ServiceResponseData<RideRequest>> GetRideRequestData(Guid requestID)
        {
            var result = await _context.RideRequests.AsNoTracking().Include(x => x.Location).Include(x => x.Passenger).FirstOrDefaultAsync(x => x.ID == requestID);
            if (result == null)
            {
                return new ServiceResponseData<RideRequest>()
                {
                    Message = "invalid_id",
                };
            }

            return new ServiceResponseData<RideRequest>()
            {
                Data = result,
                Success = true,
                Message = "all_fetched"
            };
        }

        public async Task<ServiceResponseMessage> StartRide(Guid requestID, string StartLocationName, string StartLocationCoordinates, decimal StartLatitude, decimal StartLongitude)
        {
            var result = await _context.RideRequests.Include(x => x.Location).FirstOrDefaultAsync(x => x.ID == requestID);
            if (result == null)
            {
                return new ServiceResponseMessage()
                {
                    Message = "invalid_id",
                };
            }

            if (result.RideStatus != RideStatusEnum.Accepted)
            {
                return new ServiceResponseMessage()
                {
                    Message = "cannot_start_a_non_accepted_ride",
                };
            };

            result.RideStatus = RideStatusEnum.OnRide;
            result.StartedOnDate = DateTime.Now;
            result.Location.StartLatitude = StartLatitude;
            result.Location.StartLongitude = StartLongitude;
            result.Location.StartLocationName = StartLocationName;
            result.Location.StartLocationCoordinates = StartLocationCoordinates;

            _context.RideRequests.Update(result);

            await _context.SaveChangesAsync();
            return new ServiceResponseMessage()
            {
                Message = "ride_started",
                Success = true
            };
        }

        public async Task<ServiceResponseData<SubmitRideRequestResponseDTO>> SubmitRideRequest(RideRequest ride, Guid PassengerID, Guid CarTypeID)
        {
            var passenger = await _context.Passengers.Where(x => x.ID == PassengerID).FirstOrDefaultAsync();
            if (passenger == null)
            {
                return new ServiceResponseData<SubmitRideRequestResponseDTO>()
                {
                    Message = "invalid_passenger_id"
                };
            }

            var carType = await _context.CarTypes.Where(x => x.Id == CarTypeID).FirstOrDefaultAsync();
            if (carType == null)
            {
                return new ServiceResponseData<SubmitRideRequestResponseDTO>()
                {
                    Message = "invalid_car_type_id"
                };
            }

            ride.RequestedOnDate = DateTime.Now;
            ride.RideStatus = RideStatusEnum.Pending;
            ride.Passenger = passenger;
            ride.CarType = carType;

            await _context.RideRequests.AddAsync(ride);
            await _context.SaveChangesAsync();
            var value =
                $"FROM: {ride.Location.StartLocationName}<!> " +
                $"TO:{ride.Location.DestinationName}<!> " +
                $"Passenger: {ride.Passenger.FirstName} {ride.Passenger.LastName}<!> " +
                $"Passenger_Phone_Number: {ride.Passenger.PhoneNumber}";
            Console.WriteLine(value);

            return new ServiceResponseData<SubmitRideRequestResponseDTO>()
            {
                Message = "all_fetched",
                Success = true,
                Data = new SubmitRideRequestResponseDTO()
                {
                    RequestID = ride.ID,
                    DestinationLatitude = ride.Location.DestinationLatitude,
                    DestinationLongitude = ride.Location.DestinationLongitude,
                    DestinationName = ride.Location.DestinationName,
                    StartLatitude = ride.Location.StartLatitude,
                    StartLocationName = ride.Location.StartLocationName,
                    StartLongitude = ride.Location.StartLongitude
                }
            };
        }




        private decimal CalculateTotalFare(decimal carBasePrice, decimal distanceRate, decimal timeRate, double distance, double totalRideTime)
        {
            //Fare = BasePrice + DistanceRate * Distance + TimeRate * Time
            var totalRate = (carBasePrice + distanceRate) * ((decimal)distance + timeRate) * (decimal)totalRideTime;
            return totalRate;
        }

    }
}
