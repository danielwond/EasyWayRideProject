using EasyWayRide.Services.Services.EncryptionService;
using System.IdentityModel.Tokens.Jwt;

namespace EasyWayRide.API.Helpers
{
    public static class JwtHelper
    {
        public static JwtClaims ExtractClaims(string jwtToken)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwt = jwtHandler.ReadJwtToken(jwtToken);

            var claims = new JwtClaims();

            foreach (var claim in jwt.Claims)
            {
                claims.AddClaim(claim.Type, claim.Value);
            }

            return claims;
        }

        public static Guid? GetUserID(HttpRequest request, IEncryptionService encryptionService, string Passphrase)
        {
            string authorizationHeader = request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return null;
            }

            string jwtToken = authorizationHeader["Bearer ".Length..];

            JwtClaims claims = ExtractClaims(jwtToken);

            var encrypted = claims.GetClaim("_ss_");

            var decrypted = encryptionService.DecryptString(Passphrase, encrypted);
            Guid userId = Guid.Parse(decrypted);
            return userId;
        }
    }

    public class JwtClaims
    {
        private readonly Dictionary<string, string> _claims = new Dictionary<string, string>();

        public void AddClaim(string type, string value)
        {
            _claims[type] = value;
        }

        public string GetClaim(string type)
        {
            return _claims.ContainsKey(type) ? _claims[type] : null;
        }
    }
}
