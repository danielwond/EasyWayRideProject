namespace EasyWayRide.Shared.DTOs.Driver
{
    public class RegisterDriverRequestDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string FCMToken { get; set; }
    }
}
