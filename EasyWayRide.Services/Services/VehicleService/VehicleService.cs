using EasyWayRide.DataAccess.Repos.IVehicleRepo;
using EasyWayRide.Shared.DTOs.Google;
using EasyWayRide.Shared.DTOs.Vehicle;
using EasyWayRide.Shared.Options;
using EasyWayRide.Shared.Responses;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EasyWayRide.Services.Services.VehicleService
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly HttpClient _httpClient;
        private readonly GoogleOptions _googleOptions;

        public VehicleService(IVehicleRepository vehicleRepository, IOptions<GoogleOptions> googleOptions, HttpClient httpClient)
        {
            _vehicleRepository = vehicleRepository;
            _httpClient = httpClient;
            _googleOptions = googleOptions.Value;
        }

        public async Task<ServiceResponseData<List<GetVehicleTypesDTO>>> GetCarTypesForRideRequest(decimal startLongitude, decimal startLatitude, decimal endLongitude, decimal endLatitude)
        {
            double distance = await CalculateDistance(startLongitude, startLatitude, endLongitude, endLatitude);
            return await _vehicleRepository.GetCarTypesForRideRequest(distance);
        }
        public async Task<double> CalculateDistance(decimal startLongitude, decimal startLatitude, decimal endLongitude, decimal endLatitude)
        {
            // The origin and destination latitude and longitude coordinates.
            string origin = $"{startLatitude},{startLongitude}";
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

            // Get the distance between the origin and destination.
            if (distanceMatrixResponse != null && rows != null && elements != null)
            {
                distance = elements.Distance.Value;
            }

            // Display the distance in kilometers.

            return Convert.ToDouble(distance) / 1000;
        }

    }
}
