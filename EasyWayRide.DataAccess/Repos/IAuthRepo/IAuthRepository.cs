using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.Shared.Responses;

namespace EasyWayRide.DataAccess.Repos.IAuthRepo
{
    public interface IAuthRepository
    {
        Task<ServiceResponseData<Driver?>> LoginDriver(string phoneNumber);
        Task<ServiceResponseData<Passenger?>> LoginPassenger(string phoneNumber);
    }
}
