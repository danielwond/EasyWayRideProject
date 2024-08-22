using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Shared.DTOs.Vehicle
{
    public class RegisterVehicleDTO
    {
        public string LibreImage { get; set; } = string.Empty;
        public string PlateNumber { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public string CarType { get; set; } = string.Empty;
        public string CarColor { get; set; } = string.Empty;
    }
}
