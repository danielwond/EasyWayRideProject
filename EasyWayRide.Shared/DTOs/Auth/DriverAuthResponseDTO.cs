using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Shared.DTOs.Auth
{
    public class DriverAuthResponseDTO
    {
        public string? Token { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
    }
}
