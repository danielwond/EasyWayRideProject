using EasyWayRide.API.Helpers;
using EasyWayRide.DataAccess.Hubs;
using EasyWayRide.Services.Services.DriverService;
using EasyWayRide.Services.Services.EncryptionService;
using EasyWayRide.Shared.DTOs.Driver;
using EasyWayRide.Shared.DTOs.Vehicle;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;

namespace EasyWayRide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly IEncryptionService _encryptionService;
        private readonly IHubContext<DriverHub> _driver;

        public DriverController(
            IDriverService driverService, IEncryptionService encryptionService, IHubContext<DriverHub> driver)
        {
            _driverService = driverService;
            _encryptionService = encryptionService;
            _driver = driver;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterDriver(RegisterDriverRequestDTO driver)
        {
            var result = await _driverService.RegisterDriver(driver);
            return Ok(result);
        }
        [HttpGet("get/all")]
        public async Task<IActionResult> GetAllDrivers()
        {
            var result = await _driverService.GetAllDrivers();
            return Ok(result);
        }
        [HttpPost("status/change")]
        public async Task<IActionResult> ChangeDriverOnlineStatus(ChangeDriverOnlineStatusRequestDTO data)
        {
            var userId = JwtHelper.GetUserID(Request, _encryptionService, "DriverPassphrase");
            if (userId == null)
            {
                return BadRequest();
            }
            var result = await _driverService.ChangeDriverOnlineStatus(userId.Value, data.longitude, data.latitude);
            return Ok(result);
        }
        [HttpGet("online")]
        public async Task<IActionResult> GetAllOnlineDrivers()
        {
            var result = await _driverService.GetAllOnlineDrivers();
            return Ok(result);
        }
        [HttpPost("vehicle/register")]
        public async Task<IActionResult> RegisterVehicle(Guid driverID, RegisterVehicleDTO vehicle)
        {
            var result = await _driverService.RegisterVehicle(driverID, vehicle);
            return Ok(result);
        }
        [HttpGet("vehicle/get/all")]
        public async Task<IActionResult> GetAllVehiclesByDriver(Guid driverID)
        {
            var result = await _driverService.GetAllVehiclesByDriver(driverID);
            return Ok(result);
        }
        [HttpPatch("vehicletype/add")]
        public async Task<IActionResult> AddVehicleTypeID(AddVehicleTypeRequestDTO data)
        {
            var result = await _driverService.AddVehicleTypeID(data.carTypeID, data.vehicleID);
            if (result.Success) { return Ok(result); };
            return BadRequest(result);
        }
        [HttpGet("onlinestatus/getbytoken")]
        public async Task<IActionResult> GetOnlineStatusByToken(string token)
        {
            var id = Guid.Parse(new Guid().ToString());

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            // Get the claims from the decoded token.
            var claims = securityToken.Claims;

            var claim = claims.Where(x => x.Type == "_ss_").FirstOrDefault().Value;

            var decrypted = _encryptionService.DecryptString("DriverPassphrase", claim);
            id = Guid.Parse(decrypted);

            var result = await _driverService.GetDriverDetails(id);
            return Ok(result);
        }

        [HttpGet("onlinestatus/get")]
        public async Task<IActionResult> GetOnlineStatus()
        {
            var userId = JwtHelper.GetUserID(Request, _encryptionService, "DriverPassphrase");

            var result = await _driverService.GetDriverDetails(userId.Value);
            return Ok(result);
        }

        [HttpGet("notify")]
        public async Task<IActionResult> NotifyChanged()
        {
            var drivers = await _driverService.GetAllOnlineDrivers();

            await _driver.Clients.All.SendAsync("GetOnlineDrivers", drivers);

            return Ok();
        }
        [HttpGet("location/get")]
        public async Task<IActionResult> GetOnlineStatus(Guid driverId)
        {
            var result = await _driverService.GetDriverLocationByID(driverId);
            return Ok(result);
        }
        [HttpPost("fcm/update")]
        public async Task<IActionResult> UpdateFcmToken(string fcmToken)
        {
            var driverId = JwtHelper.GetUserID(Request, _encryptionService, "DriverPassphrase");
            await _driverService.updateDriverFCMToken(driverId.ToString(), fcmToken);
            return Ok();
        }

    }
}
