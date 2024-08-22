using EasyWayRide.DataAccess.Repos.IDriverRepo;
using EasyWayRide.Shared.DTOs.Driver;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;

namespace EasyWayRide.DataAccess.Hubs
{
    public class DriverHub : Hub
    {
        private readonly IDriverRepository _repo;
        private readonly IConfiguration _configuration;

        public DriverHub(IDriverRepository repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("User Connected");
            return base.OnConnectedAsync();
        }
        public async Task SendDriverLocation(DriverLocation driverLocation)
        {
            Console.WriteLine(driverLocation);

            var driverId = GetUserID(driverLocation.driverToken);

            await _repo.UpdateDriverLocation(driverId.Value, driverLocation);

            Console.WriteLine($"UPDATED: {JsonConvert.SerializeObject(driverLocation)}");

            await Clients.All.SendAsync("DriverLocationUpdated", driverLocation);
        }


        public Guid? GetUserID(string jwtToken)
        {
            JwtClaims claims = ExtractClaims(jwtToken);

            var encrypted = claims.GetClaim("_ss_");
            var passPhrase = "DriverPassphrase";

            var decrypted = DecryptString(passPhrase, encrypted);
            Guid userId = Guid.Parse(decrypted);
            return userId;
        }
        private class JwtClaims
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
        private JwtClaims ExtractClaims(string jwtToken)
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
        private string DecryptString(string type, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using Aes aes = Aes.Create();
            var key = _configuration[$"Encryption:{type}"];
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new(buffer);
            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);
            return streamReader.ReadToEnd();
        }

    }
}
