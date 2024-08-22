using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Shared.DTOs.Driver
{
    public class AddVehicleTypeRequestDTO
    {
        public Guid carTypeID { get; set; }
        public Guid vehicleID { get; set; }
    }
}
