using EasyWayRide.DataAccess.Data;
using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.DataAccess.Repos.ICachingRepo;
using EasyWayRide.Shared.DTOs.Driver;
using EasyWayRide.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace EasyWayRide.DataAccess.Repos.IDriverRepo
{
    public class DriverRepository : IDriverRepository
    {
        private readonly EasyWayRideDataContext _context;
        private readonly EasyWayRideDapperContext _dapperContext;
        private readonly IConfiguration _configuration;
        private readonly ICachingRepository _cachingRepository;
        private readonly IDatabase _database;

        public DriverRepository(EasyWayRideDataContext context,
            EasyWayRideDapperContext dapperContext, IConfiguration configuration, ICachingRepository cachingRepository)
        {
            _context = context;
            _dapperContext = dapperContext;
            _configuration = configuration;
            _cachingRepository = cachingRepository;

            _database = cachingRepository.InitializeRedis().GetDatabase();
        }

        public async Task<ServiceResponseData<string>> ChangeDriverOnlineStatus(Guid driverID, double longitude, double latitude)
        {
            var driver = await _context.Drivers
                .Include(x => x.Vehicle)
                .ThenInclude(x => x.CarType)
                .Where(x => x.ID == driverID).FirstOrDefaultAsync();

            if (driver == null)
            {
                throw new NullReferenceException("driver doesnt exist");
            }
            else if (driver.Vehicle == null)
            {
                throw new NullReferenceException("driver doesnt have a vehicle");
            }
            else if (driver.Vehicle.CarType == null)
            {
                throw new NullReferenceException("driver's vehicle doesnt have a car type");
            }
            else
            {

                var driverId = driver.ID.ToString();
                var driver_hash = await _database.HashGetAllAsync($"online-drivers:{driverId}");

                //remove driver if he exists.
                if (driver_hash.Length != 0)
                {
                    await _database.KeyDeleteAsync($"online-drivers:{driverId}");
                    await _database.GeoRemoveAsync("locations", driverId);
                    //await _database.SortedSetRemoveAsync("locations", driverId);

                    driver.LastOffline = DateTime.Now;

                    _context.Drivers.Update(driver);
                    await _context.SaveChangesAsync();

                    return new ServiceResponseData<string>()
                    {
                        Success = true,
                        Data = "now_offline",
                        Message = "all_done",
                    };
                }
                else
                {
                    //Add the driver if he doesnt exist
                    var hash = new HashEntry[]
                    {
                        new HashEntry ("driverId" , driverId),
                        new HashEntry("OnRide", false),
                        new HashEntry("CarTypeID", driver.Vehicle.CarType.Id.ToString()),
                        new HashEntry("token", driver.FCMToken)
                    };

                    await _database.GeoAddAsync($"locations", longitude, latitude, driverId);
                    await _database.HashSetAsync($"online-drivers:{driverId}", hash);

                    driver.LastOnline = DateTime.Now;
                    _context.Drivers.Update(driver);
                    await _context.SaveChangesAsync();
                    return new ServiceResponseData<string>()
                    {
                        Success = true,
                        Data = "now_online",
                        Message = "all_done",
                    };

                    //await GetDriversWithinRadiusAsync(latitude: 8.9887, longitude: 38.7916, radius: 50);
                }

            }
        }

        public async Task<IEnumerable<string>> GetDriversWithinRadiusAsync(double latitude, double longitude, double radius)
        {
            var searchResults = await _database.GeoRadiusAsync("locations", longitude, latitude, radius, GeoUnit.Kilometers);
            var drivers = new List<string>();
            foreach (var driver in searchResults)
            {
                Console.WriteLine(driver.Member.ToString());
                Console.WriteLine(driver.Distance.ToString());
            }
            return drivers;
        }
        public async Task<ServiceResponseData<List<Driver>>> GetAllDrivers()
        {
            var result = await Task.FromResult(_context.Drivers.Include(x => x.Vehicle).AsQueryable());
            return new ServiceResponseData<List<Driver>>
            {
                Data = result.ToList(),
                Message = "all_fetched",
                Success = true
            };
        }

        public async Task<IQueryable<Vehicle>> GetAllVehiclesByDriver(Guid driverID)
        {
            var result = _context.Vehicles.AsQueryable();
            return await Task.FromResult(result);
        }

        public async Task<ServiceResponseData<string>> RegisterDriver(Driver driver)
        {
            var result = await _context.Drivers.Include(x => x.Vehicle).Where(x => x.PhoneNumber == driver.PhoneNumber).FirstOrDefaultAsync();
            if (result == null)
            {
                var isDev = bool.Parse(_configuration["Environment:isDev"]);

                if (isDev)
                {
                    var vehicle = new Vehicle()
                    {
                        CarBrand = "string",
                        CarColor = "string",
                        CarModel = "string",
                        LibreImage = "string",
                        PlateNumber = "string",
                        CarType = await _context.CarTypes.Where(x => x.Name == "Economy").FirstOrDefaultAsync()
                    };

                    await _context.Vehicles.AddAsync(vehicle);

                    driver.Vehicle = vehicle;
                }
                await _context.Drivers.AddAsync(driver);
                await _context.SaveChangesAsync();

                return new ServiceResponseData<string>()
                {
                    Message = "driver_registered",
                    Success = true
                };
            }
            else
            {
                return new ServiceResponseData<string>()
                {
                    Message = "phone_number_exists"
                };
            }
        }

        public async Task<ServiceResponseMessage> RegisterVehicle(Guid driverID, Vehicle vehicle)
        {
            //Look for driver
            var driver = await _context.Drivers.Where(x => x.ID == driverID).FirstOrDefaultAsync();

            if (driver == null)
            {
                return new ServiceResponseMessage()
                {
                    Message = "invalid_driver_id"
                };
            }
            else if (driver.Vehicle != null)
            {
                return new ServiceResponseMessage()
                {
                    Message = "vehicle_already_registered"
                };
            }
            //add vehicle to db
            await _context.Vehicles.AddAsync(vehicle);

            //assign vehicle to driver
            driver.Vehicle = vehicle;

            //save changes
            await _context.SaveChangesAsync();

            return new ServiceResponseMessage()
            {
                Message = "vehicle_added",
                Success = true
            };
        }

        public async Task<ServiceResponseMessage> AddVehicleTypeID(Guid carTypeID, Guid vehicleID)
        {
            var carTypeId = await _context.CarTypes.Where(x => x.Id == carTypeID).FirstOrDefaultAsync();
            if (carTypeId == null)
            {
                return new ServiceResponseMessage()
                {
                    Message = "invalid_car_type_id",

                };
            }
            var vehicle = await _context.Vehicles.Where(x => x.ID == vehicleID).FirstOrDefaultAsync();
            if (vehicle == null)
            {
                return new ServiceResponseMessage()
                {
                    Message = "invalid_vehicle_id",
                };
            }

            vehicle.CarType = carTypeId;

            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();

            return new ServiceResponseMessage()
            {
                Message = "all_done",
                Success = true
            };
        }

        public async Task<ServiceResponseData<string>> GetOnlineDrivers()
        {
            var hashFields = await _database.HashGetAllAsync("online-drivers:*");
            if (hashFields.Length != 0)
            {
                return new ServiceResponseData<string>()
                {
                    Data = string.Join(",", hashFields),
                    Message = "all-fetched",
                    Success = true
                };
            }
            else
            {
                return new ServiceResponseData<string>()
                {
                    Message = "no_online_drivers"
                };
            }
        }

        public async Task<ServiceResponseData<string>> GetDriverDetails(Guid DriverID)
        {
            var onlineDriverKey = $"online-drivers:{DriverID}";
            var onlineDriverExists = await _database.KeyExistsAsync(onlineDriverKey);
            var location = await _database.GeoPositionAsync("locations", DriverID.ToString());

            string result = onlineDriverExists && location.HasValue ? "true" : "false";

            return new ServiceResponseData<string>()
            {
                Data = result,
                Message = "all_fetched",
                Success = true
            };
        }

        public async Task<ServiceResponseData<string>> UpdateDriverLocation(Guid driverId, DriverLocation driverLocation)
        {
            var driver = await _context.Drivers
                .Include(x => x.Vehicle)
                .ThenInclude(x => x.CarType)
                .Where(x => x.ID == driverId).FirstOrDefaultAsync();

            if (driver == null)
            {
                throw new NullReferenceException("driver doesnt exist");
            }
            else if (driver.Vehicle == null)
            {
                throw new NullReferenceException("driver doesnt have a vehicle");
            }
            else if (driver.Vehicle.CarType == null)
            {
                throw new NullReferenceException("driver's vehicle doesnt have a car type");
            }
            else
            {

                await _database.GeoAddAsync($"locations", driverLocation.Longitude, driverLocation.Latitude, driverId.ToString());

                return new ServiceResponseData<string>()
                {
                    Success = true,
                    Data = "location_updated",
                    Message = "all_done",
                };
            }
        }

        public async Task<ServiceResponseData<Dictionary<string, string>>> GetDriverLocationByID(Guid driverId)
        {
            var result = await _database.GeoPositionAsync("locations", driverId.ToString());

            if (result.HasValue)
            {
                var longitude = result.Value.Longitude;
                var latitude = result.Value.Latitude;

                // Use the longitude and latitude values as needed
                return new ServiceResponseData<Dictionary<string, string>>()
                {
                    Data = new Dictionary<string, string>()
                    {
                        {"longitude" ,longitude.ToString() },
                        {"latitude" ,latitude.ToString() },
                    },
                    Message = "all_fetched",
                    Success = true
                };
            }
            else
            {
                return new ServiceResponseData<Dictionary<string, string>>()
                {
                    Message = "driver_doesnt_exist"
                };
            }
        }

        public async Task UpdateDriverFCMToken(string driverId, string fcmToken)
        {
            var driver = await _context.Drivers.Where(x => x.ID == Guid.Parse(driverId)).FirstOrDefaultAsync();
            if (driver == null)
            {
                throw new Exception("Invalid driver Id");
            }
            else if (driver.FCMToken == fcmToken)
            {
                return;
            }
            else
            {
                driver.FCMToken = fcmToken;
                _context.Drivers.Update(driver);
                await _context.SaveChangesAsync();

                return;
            }
        }
    }
}
