using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Shared.DTOs.Ride
{
    public class CancelRideRequestDTO
    {
        public Guid requestID { get; set; }
        public string? cancellationReason { get; set; }
    }
}
