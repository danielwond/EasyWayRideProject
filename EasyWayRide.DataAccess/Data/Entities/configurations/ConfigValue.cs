using EasyWayRide.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWayRide.DataAccess.Data.Entities.configurations
{
    [Table("TimingData", Schema = "Configuration")]

    public class ConfigValue
    {
        public int ID { get; set; }
        public ConfigNameEnumId Name { get; set; }
        public string? Description { get; set; }
        public decimal Value { get; set; }
        public MeasurementEnumId Measurement { get; set; }
    }
}
