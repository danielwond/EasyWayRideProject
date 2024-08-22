using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.DataAccess.Data.Entities.configurations
{
    [Table("FareData", Schema = "Configuration")]
    public class FareData
    {
        public int ID { get; set; }
        public decimal PricePerMeter { get; set; }
    }
}
