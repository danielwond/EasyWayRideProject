using EasyWayRide.Shared.Enums;

namespace EasyWayRide.DataAccess.Data.Entities.dbo
{
    public class Passenger
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? FcmToken { get; set; }
        public AccountStatusEnum AccountStatus { get; set; }
    }
}
