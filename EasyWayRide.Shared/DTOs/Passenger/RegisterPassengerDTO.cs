using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Shared.DTOs.Passenger
{
    public class RegisterPassengerDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string FCMToken { get; set; }
    }
}
