using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Shared.DTOs.Ride
{
    public class StartRideRequestDTO
    {
        public Guid requestID { get; set; }
        public string StartLocationName { get; set; } = string.Empty;
        public string StartLocationCoordinates { get; set; } = string.Empty;
        public decimal StartLatitude { get; set; } = decimal.Zero;
        public decimal StartLongitude { get; set; } = decimal.Zero;
    }
}
