using EasyWayRide.DataAccess.Data;
using EasyWayRide.Shared.DTOs.Vehicle;
using EasyWayRide.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace EasyWayRide.DataAccess.Repos.IVehicleRepo
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly EasyWayRideDataContext _context;

        public VehicleRepository(EasyWayRideDataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponseMessage> AddCarTypeID(Guid carTypeID, Guid vehicleID)
        {
            var carTypeId = await _context.CarTypes.Where(x => x.Id == carTypeID).FirstOrDefaultAsync();
            if (carTypeId == null)
            {
                return new ServiceResponseMessage()
                {
                    Message = "invalid_car_type_id",

                };
            }
            var vehicle = await _context.Vehicles.Where(x => x.ID == vehicleID).FirstOrDefaultAsync();
            if (vehicle == null)
            {
                return new ServiceResponseMessage()
                {
                    Message = "invalid_vehicle_id",
                };
            }

            vehicle.CarType = carTypeId;

            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();

            return new ServiceResponseMessage()
            {
                Message = "all_done",
                Success = true
            };
        }

        public async Task<ServiceResponseData<List<GetVehicleTypesDTO>>> GetCarTypesForRideRequest(double distance)
        {
            var carTypes = await Task.FromResult(_context.CarTypes.AsQueryable());
            if (carTypes != null)
            {

                var result = new List<GetVehicleTypesDTO>();
                foreach (var carType in carTypes)
                {
                    //fare = baseFare + (costPerMile * distance) + (costPerMinute * time)
                    var vehicleType = new GetVehicleTypesDTO()
                    {
                        Name = carType.Name,
                        Price = carType.BasePrice + carType.PerKmRate * (decimal)distance + carType.PerMinuteRate,
                        CarTypeID = carType.Id.ToString()
                    };
                    result.Add(vehicleType);
                }
                return new ServiceResponseData<List<GetVehicleTypesDTO>>()
                {
                    Data = result,
                    Message = "all_fetched",
                    Success = true
                };
            }
            return new ServiceResponseData<List<GetVehicleTypesDTO>>() { Message = "empty_car_types" };
        }

    }
}
