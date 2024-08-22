using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.DataAccess.Data.Entities.configurations
{
    [Table("RideTypes", Schema = "Configuration")]

    public record CarType
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; } = decimal.Zero;
        public decimal PerKmRate { get; set; } = decimal.Zero;
        public decimal PerMinuteRate { get; set; } = decimal.Zero;
        public decimal CancellationFee { get; set; } = decimal.Zero;
        public bool IsActive { get; set; }
    }
}
