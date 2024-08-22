using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Shared.DTOs.Ride
{
    public class EndRideRequestDTO
    {
        public Guid requestID { get; set; }
        public string EndLocationName { get; set; } = string.Empty;
        public string EndLocationCoordinates { get; set; } = string.Empty;
        public decimal EndLatitude { get; set; } = decimal.Zero;
        public decimal EndLongitude { get; set; } = decimal.Zero;
    }
}
