namespace EasyWayRide.Shared.DTOs.Passenger
{
    public class GetDriversWithInARadiusDto
    {
        public int numberOfDrivers { get; set; }
        public List<DriverInfo>? Drivers { get; set; }
    }
    public class DriverInfo
    {
        public string? GuidId { get; set; }
        public string? Distance { get; set; }
        public DriverInfoPosition Position { get; set; }
        public string? FcmToken { get; set; }
        public bool? onRide { get; set; }
        public string? CarTypeID { get; set; }
    }
    public class DriverInfoPosition
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
