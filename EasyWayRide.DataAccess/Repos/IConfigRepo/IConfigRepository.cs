using EasyWayRide.DataAccess.Data.Entities.configurations;
using EasyWayRide.Shared.Responses;

namespace EasyWayRide.DataAccess.Repos.IConfigRepo
{
    public interface IConfigRepository
    {
        public Task<ServiceResponseMessage> InsertCarType(CarType carType);
        public Task<ServiceResponseData<List<CarType>>> GetAllCarTypes();

        public Task<ServiceResponseData<List<ConfigValue>>> GetAllConfigValues();
    }
}
