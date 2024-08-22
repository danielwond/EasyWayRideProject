using EasyWayRide.API.Helpers;
using EasyWayRide.Services.Services.EncryptionService;
using EasyWayRide.Services.Services.RideService;
using EasyWayRide.Services.Services.VehicleService;
using EasyWayRide.Shared.DTOs.Ride;
using Microsoft.AspNetCore.Mvc;

namespace EasyWayRide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IRideService _rideService;
        private readonly IEncryptionService _encryptionService;

        public RideController(IVehicleService vehicleService, IRideService rideService, IEncryptionService encryptionService)
        {
            _vehicleService = vehicleService;
            _rideService = rideService;
            _encryptionService = encryptionService;
        }
        [HttpPost("cartypes/get")]
        public async Task<IActionResult> GetCarTypesForRideRequest(GetCarTypesForRideRequestDTO data)
        {
            var result = await _vehicleService.GetCarTypesForRideRequest(data.startLongitude, data.startLatitude, data.endLongitude, data.endLatitude);
            if (result.Success) { return Ok(result); }
            return BadRequest(result);
        }
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitRideRequest(SubmitRideRequestDTO data)
        {
            var PassengerID = JwtHelper.GetUserID(Request, _encryptionService, "PassengerPassphrase");
            if (PassengerID == null) { return BadRequest("invalid token"); }

            var result = await _rideService.SubmitRideRequest(data.ride, PassengerID.Value, data.CarTypeID);
            if (result.Success == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("accept")]
        public async Task<IActionResult> AcceptRide(Guid requestID)
        {
            var driverId = JwtHelper.GetUserID(Request, _encryptionService, "DriverPassphrase");
            if (driverId == null)
            {
                return BadRequest("invalid_driver_id");
            }
            var result = await _rideService.AcceptRide(requestID, driverId.Value);
            if (result.Success == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("start")]
        public async Task<IActionResult> StartRide(StartRideRequestDTO data)
        {
            var result = await _rideService.StartRide(data.requestID, data.StartLocationName, data.StartLocationCoordinates, data.StartLatitude, data.StartLongitude);
            if (result.Success == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("end")]
        public async Task<IActionResult> EndRide(EndRideRequestDTO data)
        {
            var result = await _rideService.EndRide(data.requestID, data.EndLocationName, data.EndLocationCoordinates, data.EndLatitude, data.EndLongitude);
            if (result.Success == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("cancel")]
        public async Task<IActionResult> CancelRide(CancelRideRequestDTO data)
        {
            var result = await _rideService.CancelRide(data.requestID, data.cancellationReason);
            if (result.Success == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
