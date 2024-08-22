using EasyWayRide.Shared.DTOs.Vehicle;
using EasyWayRide.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.DataAccess.Repos.IVehicleRepo
{
    public interface IVehicleRepository
    {
        public Task<ServiceResponseData<List<GetVehicleTypesDTO>>> GetCarTypesForRideRequest(double distance);
    }
}
