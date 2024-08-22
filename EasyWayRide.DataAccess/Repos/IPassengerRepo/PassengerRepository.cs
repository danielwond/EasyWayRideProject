using EasyWayRide.DataAccess.Data;
using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.DataAccess.Repos.ICachingRepo;
using EasyWayRide.Shared.DTOs.Passenger;
using EasyWayRide.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace EasyWayRide.DataAccess.Repos.IPassengerRepo
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly EasyWayRideDataContext _context;
        private readonly ICachingRepository _cachingRepository;
        private readonly IDatabase _database;

        public PassengerRepository(EasyWayRideDataContext context, ICachingRepository cachingRepository)
        {
            _context = context;
            _cachingRepository = cachingRepository;
            _database = _cachingRepository.InitializeRedis().GetDatabase();
        }

        public async Task<IQueryable<Passenger>> GetAllPassengers()
        {
            var result = _context.Passengers.AsQueryable();
            return await Task.FromResult(result);

        }

        public async Task<GetDriversWithInARadiusDto> GetDriversWithinRadiusAsync(double latitude, double longitude)
        {
            var radius = await _context.ConfigValues.Where(x => x.Name == Shared.Enums.ConfigNameEnumId.searching_radius).FirstOrDefaultAsync();
            radius ??= new Data.Entities.configurations.ConfigValue()
            {
                Value = 25
            };
            var searchResults = await _database.GeoRadiusAsync("locations", longitude, latitude, (int)radius.Value, GeoUnit.Kilometers);
            var drivers = new List<DriverInfo>();

            if (searchResults.Length > 0)
            {
                drivers = searchResults.Select(async result => new DriverInfo
                {
                    Distance = result.Distance.ToString(),
                    GuidId = result.Member.ToString(),
                    Position = new DriverInfoPosition()
                    {
                        latitude = result.Position!.Value.Latitude,
                        longitude = result.Position!.Value.Longitude,
                    },
                    FcmToken = await _database.HashGetAsync($"online-drivers:{result.Member}", "token"),
                    onRide = await GetOnRideProperty(result),
                    CarTypeID = await _database.HashGetAsync($"online-drivers:{result.Member}", "CarTypeID")

                }).Select(driverTask => driverTask.Result).ToList();
            }

            return new GetDriversWithInARadiusDto()
            {
                Drivers = drivers,
                numberOfDrivers = drivers.Count
            };
        }

        public async Task<ServiceResponseMessage> RegisterPassenger(Passenger passenger)
        {
            var result = await _context.Passengers.Where(x => x.PhoneNumber == passenger.PhoneNumber).FirstOrDefaultAsync();
            if (result == null)
            {
                await _context.Passengers.AddAsync(passenger);
                await _context.SaveChangesAsync();

                return new ServiceResponseMessage()
                {
                    Message = "passenger_registered",
                    Success = true
                };
            }
            else
            {
                return new ServiceResponseMessage()
                {
                    Message = "phone_number_exists"
                };
            }
        }

        private async Task<bool?> GetOnRideProperty(GeoRadiusResult result)
        {
            var value = await _database.HashGetAsync($"online-drivers:{result.Member}", "OnRide");
            if (value.IsNullOrEmpty)
            {
                return null;
            }
            return int.Parse(value!) != 0;
        }
    }
}
