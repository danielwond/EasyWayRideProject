using EasyWayRide.Services.Services.ConfigService;
using EasyWayRide.Shared.DTOs.IConfig;
using Microsoft.AspNetCore.Mvc;

namespace EasyWayRide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigService _configService;

        public ConfigController(IConfigService configService)
        {
            _configService = configService;
        }
        [HttpPost("cartype/add")]
        public async Task<IActionResult> AddCarType(AddCarTypeRequestDto carType)
        {
            var result = await _configService.InsertCarType(carType);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("cartype/getall")]
        public async Task<IActionResult> GetAllCarTypes()
        {
            var result = await _configService.GetCarTypes();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
