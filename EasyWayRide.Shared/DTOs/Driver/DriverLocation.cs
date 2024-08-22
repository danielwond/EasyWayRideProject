namespace EasyWayRide.Shared.DTOs.Driver
{
    public record DriverLocation
    {
        public string? driverToken { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
