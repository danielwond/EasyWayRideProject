using EasyWayRide.DataAccess.Repos.IAuthRepo;
using EasyWayRide.Services.Services.TokenService;
using EasyWayRide.Shared.Responses;

namespace EasyWayRide.Services.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IAuthRepository authRepository, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
        }

        public async Task<ServiceResponseBase> LoginDriver(string phoneNumber)
        {
            var result = await _authRepository.LoginDriver(phoneNumber);

            if (result.Success && result.Data != null)
            {
                return new ServiceResponseData<string>()
                {
                    Data = _tokenService.GenerateDriverToken(result.Data),
                    Message = result.Message,
                    Success = true
                };
            }
            return new ServiceResponseMessage
            {
                Message = result.Message,
            };
        }

        public async Task<ServiceResponseBase> LoginPassenger(string phoneNumber)
        {
            var result = await _authRepository.LoginPassenger(phoneNumber);

            if (result.Success && result.Data != null)
            {
                return new ServiceResponseData<string>()
                {
                    Data = _tokenService.GeneratePassengerToken(result.Data),
                    Message = result.Message,
                    Success = true
                };
            }
            return new ServiceResponseMessage
            {
                Message = result.Message,
            };
        }
    }
}
