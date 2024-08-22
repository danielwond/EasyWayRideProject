using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.Services.Helpers;
using EasyWayRide.Services.Services.EncryptionService;
using EasyWayRide.Shared.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static CoordinateSharp.GeoFence;

namespace EasyWayRide.Services.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger<ITokenService> _logger;

        public TokenService(IOptions<JwtOptions> jwtOptions, IEncryptionService encryptionService, ILogger<ITokenService> logger)
        {
            _jwtOptions = jwtOptions.Value;
            _encryptionService = encryptionService;
            _logger = logger;
        }

        public string GenerateDriverToken(Driver driver)
        {
            try
            {
                var userID = _encryptionService.EncryptString("DriverPassphrase", driver.ID.ToString());
                List<Claim> claims = new()
                    {
                        new Claim("_ss_", userID),
                        new Claim("_oo_", "d"),
                    };

                _logger.LogInformation($"driverId: {driver.ID}");

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
                var creds = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha512);
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.ToUniversalTime().AddDays(5),
                    signingCredentials: creds);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GenerateEmployeeToken(Employee emp)
        {
            try
            {
                var userID = _encryptionService.EncryptString("EmployeePassphrase", emp.Id.ToString());

                List<Claim> claims = new()
                    {
                        new Claim("_ss_", userID),
                        new Claim("_oo_", "s"),
                    };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
                var creds = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha512);
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.ToUniversalTime().AddMonths(5),
                    signingCredentials: creds);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GeneratePassengerToken(Passenger passenger)
        {
            try
            {
                //var userID = _encryptionService.EncryptString("PassengerPassphrase", passenger.ID.ToString());

                List<Claim> claims = new()
                    {
                        new Claim("_ss_", passenger.ID.ToString()),
                        new Claim("_oo_", "p"),
                    };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
                var creds = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha512);
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.ToUniversalTime().AddMonths(5),
                    signingCredentials: creds);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
