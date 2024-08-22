using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.Shared.DTOs.Passenger;
using EasyWayRide.Shared.Responses;

namespace EasyWayRide.DataAccess.Repos.IPassengerRepo
{
    public interface IPassengerRepository
    {
        public Task<IQueryable<Passenger>> GetAllPassengers();
        public Task<ServiceResponseMessage> RegisterPassenger(Passenger passenger);
        public Task<GetDriversWithInARadiusDto> GetDriversWithinRadiusAsync(double latitude, double longitude);
    }
}
