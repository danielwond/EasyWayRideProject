using EasyWayRide.Shared.Responses;

namespace EasyWayRide.Services.Services.AuthService
{
    public interface IAuthService
    {
        //Login with phone number
        Task<ServiceResponseBase> LoginDriver(string phoneNumber);
        Task<ServiceResponseBase> LoginPassenger(string phoneNumber);
    }
}
