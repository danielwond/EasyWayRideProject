using EasyWayRide.DataAccess.Data.Entities.configurations;
using EasyWayRide.Shared.DTOs.Vehicle;
using EasyWayRide.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Services.Services.VehicleService
{
    public interface IVehicleService
    {
        public Task<ServiceResponseData<List<GetVehicleTypesDTO>>> GetCarTypesForRideRequest(decimal passengerLongitude, decimal passengerLatitude, decimal driverLongitude, decimal driverLatitude);
    }
}
