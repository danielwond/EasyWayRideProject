using EasyWayRide.DataAccess.Data;
using EasyWayRide.DataAccess.Data.Entities.configurations;
using EasyWayRide.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace EasyWayRide.DataAccess.Repos.ISeedRepo
{
    public class SeedRepository : ISeedRepository
    {
        private readonly EasyWayRideDataContext _context;

        public SeedRepository(EasyWayRideDataContext context)
        {
            _context = context;
        }
        public async Task SeedCarTypes()
        {
            var result = await _context.CarTypes.AnyAsync();
            if (!result)
            {
                var car_types = new List<CarType>()
                {
                    new CarType
                    {
                        IsActive = true,
                        Name = "Economy",
                        Description = "Economy",
                        PerKmRate = 10,
                        PerMinuteRate = 5,
                        BasePrice = 10,
                        CancellationFee = 0
                    },
                    new CarType
                    {
                        IsActive = true,
                        Name = "Van",
                        Description = "Van",
                        PerKmRate = 20,
                        PerMinuteRate = 6,
                        BasePrice = 10,
                        CancellationFee = 0
                    }
                };
                await _context.CarTypes.AddRangeAsync(car_types);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SeedConfigurationData()
        {
            var result = await _context.ConfigValues.AnyAsync();
            if (!result)
            {
                var datas = new List<ConfigValue>()
                {
                    new ConfigValue()
                    {
                        Description = "Total Ride Request Waiting Time",
                        Value = 120000,
                        Name = ConfigNameEnumId.ride_waiting_time,
                        Measurement = MeasurementEnumId.Second
                    },
                    new ConfigValue()
                    {
                        Description = "Ride Request Waiting Time For Driver",
                        Value = 30000,
                        Name = ConfigNameEnumId.driver_waiting_time,
                        Measurement = MeasurementEnumId.Second
                    },
                    new ConfigValue()
                    {
                        Description = "Searching Radius Time",
                        Value = 25,
                        Name = ConfigNameEnumId.searching_radius,
                        Measurement = MeasurementEnumId.Kilometer
                    }
                };
                await _context.ConfigValues.AddRangeAsync(datas);
                await _context.SaveChangesAsync();
            }
        }
    }
}
