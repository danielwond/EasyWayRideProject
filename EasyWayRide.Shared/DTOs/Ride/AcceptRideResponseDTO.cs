using System.Text.Json.Serialization;

namespace EasyWayRide.Shared.DTOs.Ride
{
    public class DriverAcceptResponseDTO
    {
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid DriverID { get; set; }
        public Guid RequestId { get; set; }
    }
    public class VehcileAcceptResponseDTO
    {
        public string? PlateNumber { get; set; }
        public string? CarModel { get; set; }
        public string? CarBrand { get; set; }
        public string? CarColor { get; set; }
    }
    public class AcceptRideResponseDTO
    {

        [JsonPropertyName(name: "Driver")]
        public DriverAcceptResponseDTO Driver { get; set; } = new DriverAcceptResponseDTO();

        [JsonPropertyName(name: "Vehicle")]
        public VehcileAcceptResponseDTO Vehicle { get; set; } = new VehcileAcceptResponseDTO();
    }
}
