using EasyWayRide.DataAccess.Data.Entities.configurations;

namespace EasyWayRide.DataAccess.Data.Entities.dbo
{
    public class Vehicle
    {
        public Guid ID { get; set; }
        public string LibreImage { get; set; } = string.Empty;
        public string PlateNumber { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public string CarBrand { get; set; } = string.Empty;
        public string CarColor { get; set; } = string.Empty;
        public CarType? CarType { get; set; }
    }
}
