using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Shared.DTOs.Ride
{
    public class GetCarTypesForRideRequestDTO
    {
        //decimal startLongitude, decimal startLatitude, decimal endLongitude, decimal endLatitude
        public decimal startLongitude { get; set; }
        public decimal startLatitude { get; set; }
        public decimal endLongitude { get; set; }
        public decimal endLatitude { get; set; }
    }
}
