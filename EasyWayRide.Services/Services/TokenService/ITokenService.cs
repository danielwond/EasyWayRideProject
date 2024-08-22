using EasyWayRide.DataAccess.Data.Entities.dbo;

namespace EasyWayRide.Services.Services.TokenService
{
    public interface ITokenService
    {
        public string GenerateEmployeeToken(Employee emp);
        public string GenerateDriverToken(Driver driver);
        public string GeneratePassengerToken(Passenger passenger);

    }
}
