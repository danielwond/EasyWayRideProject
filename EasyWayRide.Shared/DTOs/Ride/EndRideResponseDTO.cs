using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Shared.DTOs.Ride
{
    public class EndRideResponseDTO
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal TotalFare { get; set; }
        public double TotalMinutes { get; set; }
    }
}
