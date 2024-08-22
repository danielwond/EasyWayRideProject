using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.Shared.DTOs.Driver;
using EasyWayRide.Shared.Responses;

namespace EasyWayRide.DataAccess.Repos.IDriverRepo
{
    public interface IDriverRepository
    {
        public Task<ServiceResponseData<string>> ChangeDriverOnlineStatus(Guid driverID, double longitude, double latitude);
        public Task<ServiceResponseData<string>> RegisterDriver(Driver driver);
        public Task<ServiceResponseMessage> RegisterVehicle(Guid driverID, Vehicle vehicle);
        public Task<ServiceResponseData<List<Driver>>> GetAllDrivers();
        public Task<ServiceResponseData<string>> GetOnlineDrivers();
        public Task<IQueryable<Vehicle>> GetAllVehiclesByDriver(Guid driverID);
        public Task<ServiceResponseMessage> AddVehicleTypeID(Guid carTypeID, Guid vehicleID);
        public Task<ServiceResponseData<string>> GetDriverDetails(Guid DriverID);
        public Task<ServiceResponseData<string>> UpdateDriverLocation(Guid driverId, DriverLocation driverLocation);

        public Task UpdateDriverFCMToken(string driverId, string fcmToken);
        public Task<ServiceResponseData<Dictionary<string, string>>> GetDriverLocationByID(Guid id);

    }
}
