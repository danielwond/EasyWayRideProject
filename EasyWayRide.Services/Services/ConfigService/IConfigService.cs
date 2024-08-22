using EasyWayRide.DataAccess.Data.Entities.configurations;
using EasyWayRide.Shared.DTOs.IConfig;
using EasyWayRide.Shared.DTOs.Vehicle;
using EasyWayRide.Shared.Responses;

namespace EasyWayRide.Services.Services.ConfigService
{
    public interface IConfigService
    {
        public Task<ServiceResponseMessage> InsertCarType(AddCarTypeRequestDto carType);
        public Task<ServiceResponseData<List<CarType>>> GetCarTypes();
    }
}
