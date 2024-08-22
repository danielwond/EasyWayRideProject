using AutoMapper;
using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.DataAccess.Repos.IDriverRepo;
using EasyWayRide.Services.Services.NotificationService;
using EasyWayRide.Services.Services.TokenService;
using EasyWayRide.Shared.DTOs.Driver;
using EasyWayRide.Shared.DTOs.Vehicle;
using EasyWayRide.Shared.Enums;
using EasyWayRide.Shared.Responses;
using Microsoft.Extensions.Logging;

namespace EasyWayRide.Services.Services.DriverService
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IMapper _mapper;
        private readonly IPushNotificationService _pushNotificationService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<IDriverService> _logger;

        public DriverService(IDriverRepository driverRepository,
            IMapper mapper,
            IPushNotificationService pushNotificationService,
            ITokenService tokenService, ILogger<IDriverService> logger)
        {
            _driverRepository = driverRepository;
            _mapper = mapper;
            _pushNotificationService = pushNotificationService;
            _tokenService = tokenService;
            _logger = logger;
        }
        public bool IsDriver { get; set; }
        public async Task<ServiceResponseData<string>> ChangeDriverOnlineStatus(Guid DriverID, double longitude, double latitude)
        {
            return await _driverRepository.ChangeDriverOnlineStatus(DriverID, longitude, latitude);
        }

        public async Task<ServiceResponseBase> GetAllDrivers()
        {
            return await _driverRepository.GetAllDrivers();
        }

        public async Task<ServiceResponseBase> GetAllOnlineDrivers()
        {
            var result = await _driverRepository.GetOnlineDrivers();
            return result;
        }

        public async Task<IQueryable<Vehicle>> GetAllVehiclesByDriver(Guid driverID)
        {
            var result = await _driverRepository.GetAllVehiclesByDriver(driverID);
            return await Task.FromResult(result);
        }

        public async Task<ServiceResponseBase> RegisterDriver(RegisterDriverRequestDTO driver)
        {
            try
            {
                var d_entity = _mapper.Map<Driver>(driver);

                d_entity.ID = new Guid();
                d_entity.RegisteredOn = DateTime.Now;
                d_entity.AccountStatus = AccountStatusEnum.Pending;
                d_entity.FCMToken = driver.FCMToken;
                d_entity.DateOfBirth = DateTime.Now;

                var result = await _driverRepository.RegisterDriver(d_entity);
                if (result.Success)
                {
                    result.Data = _tokenService.GenerateDriverToken(d_entity);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("RegisterDriver");
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<ServiceResponseBase> RegisterVehicle(Guid driverID, RegisterVehicleDTO vehicle)
        {
            var vehicle_map = _mapper.Map<Vehicle>(vehicle);

            vehicle_map.ID = new Guid();

            var result = await _driverRepository.RegisterVehicle(driverID, vehicle_map);
            return result;
        }
        public async Task<ServiceResponseBase> AddVehicleTypeID(Guid carTypeID, Guid vehicleID)
        {
            return await _driverRepository.AddVehicleTypeID(carTypeID, vehicleID);
        }

        public async Task<ServiceResponseBase> GetDriverDetails(Guid DriverID)
        {
            return await _driverRepository.GetDriverDetails(DriverID);
        }

        public async Task<ServiceResponseBase> GetDriverLocationByID(Guid id)
        {
            return await _driverRepository.GetDriverLocationByID(id);
        }

        public async Task updateDriverFCMToken(string driverId, string fcmToken)
        {
            await _driverRepository.UpdateDriverFCMToken(driverId, fcmToken);
        }
    }
}
