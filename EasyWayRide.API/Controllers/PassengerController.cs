using EasyWayRide.Services.Services.PassengerService;
using EasyWayRide.Shared.DTOs.Passenger;
using Microsoft.AspNetCore.Mvc;

namespace EasyWayRide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly IPassengerService _passengerService;

        public PassengerController(IPassengerService passengerService)
        {
            _passengerService = passengerService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterPassenger(RegisterPassengerDTO passenger)
        {
            return Ok(await _passengerService.RegisterPassenger(passenger));
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllPassengers()
        {
            return Ok(await _passengerService.GetAllPassengers());
        }

        [HttpGet("drivers/get")]
        public async Task<IActionResult> GetDriversWithinRadiusAsync(double longitude, double latitiude)
        {
            return Ok(await _passengerService.GetDriversWithinRadiusAsync(latitiude, longitude));
        }
    }
}
