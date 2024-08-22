using EasyWayRide.Services.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace EasyWayRide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet("driver/login")]
        public async Task<IActionResult> LoginDriver(string phoneNumber)
        {
            var result = await _authService.LoginDriver(phoneNumber);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("passenger/login")]
        public async Task<IActionResult> LoginPassenger(string phoneNumber)
        {
            var result = await _authService.LoginPassenger(phoneNumber);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
