using EasyWayRide.Shared.Enums;

namespace EasyWayRide.DataAccess.Data.Entities.dbo
{
    public class Driver
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Vehicle? Vehicle { get; set; }
        public DateTime RegisteredOn { get; set; }
        public bool RegisteredFromAdmin { get; set; }
        public AccountStatusEnum AccountStatus { get; set; }
        public DateTime LastOnline { get; set; }
        public DateTime LastOffline { get; set; }
        public string? FCMToken { get; set; }
    }
}
