using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWayRide.DataAccess.Data.Entities.configurations
{
    [Table("RideStatus", Schema = "Configuration")]

    public class RideStatus
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }
}
