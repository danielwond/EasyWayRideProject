using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.Shared.DTOs.Driver;
using EasyWayRide.Shared.DTOs.Vehicle;
using EasyWayRide.Shared.Responses;

namespace EasyWayRide.Services.Services.DriverService
{
    public interface IDriverService
    {
        public bool IsDriver { get; set; }
        public Task<ServiceResponseData<string>> ChangeDriverOnlineStatus(Guid driverID, double longitude, double latitude);
        public Task<ServiceResponseBase> RegisterDriver(RegisterDriverRequestDTO driver);
        public Task<ServiceResponseBase> GetAllDrivers();
        public Task<ServiceResponseBase> GetAllOnlineDrivers();
        public Task<ServiceResponseBase> RegisterVehicle(Guid driverID, RegisterVehicleDTO vehicle);
        public Task<ServiceResponseBase> AddVehicleTypeID(Guid carTypeID, Guid vehicleID);

        public Task<ServiceResponseBase> GetDriverDetails(Guid DriverID);
        public Task<IQueryable<Vehicle>> GetAllVehiclesByDriver(Guid driverID);
        public Task<ServiceResponseBase> GetDriverLocationByID(Guid id);
        public Task updateDriverFCMToken(string driverId, string fcmToken);
    }
}
