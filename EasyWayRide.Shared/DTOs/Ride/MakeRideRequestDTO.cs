namespace EasyWayRide.Shared.DTOs.Ride
{
    public class MakeRideRequestDTO
    {
        public string? Pickup { get; set; }
        public string? Dropoff { get; set; }
        public string? EstimatedAmount { get; set; }
        public string? Distance { get; set; }
    }
}
