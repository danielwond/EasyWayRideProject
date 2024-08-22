using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Shared.DTOs.IConfig
{
    public class AddCarTypeRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal PerKmRate { get; set; }
        public decimal PerMinuteRate { get; set; }
    }
}
