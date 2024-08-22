using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.Shared.DTOs.Passenger;
using EasyWayRide.Shared.Responses;

namespace EasyWayRide.Services.Services.PassengerService
{
    public interface IPassengerService
    {
        public Task<IEnumerable<Passenger>> GetAllPassengers();
        public Task<ServiceResponseMessage> RegisterPassenger(RegisterPassengerDTO passenger);

        public Task<GetDriversWithInARadiusDto> GetDriversWithinRadiusAsync(double latitude, double longitude);
    }
}
