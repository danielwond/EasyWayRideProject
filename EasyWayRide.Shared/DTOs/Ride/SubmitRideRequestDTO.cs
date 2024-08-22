namespace EasyWayRide.Shared.DTOs.Ride
{
    public class SubmitRideRequestDTO
    {
        public CreateRideRequestDTO ride { get; set; }
        public Guid CarTypeID { get; set; }
    }
}
