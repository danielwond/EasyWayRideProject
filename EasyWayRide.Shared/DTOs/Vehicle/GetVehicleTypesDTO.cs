using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Shared.DTOs.Vehicle
{
    public class GetVehicleTypesDTO
    {
        //TODO: Encrypt the vehicle ID of the vehicle
        public string? CarTypeID { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
