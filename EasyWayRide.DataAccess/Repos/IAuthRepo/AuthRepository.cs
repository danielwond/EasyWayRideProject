using EasyWayRide.DataAccess.Data;
using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace EasyWayRide.DataAccess.Repos.IAuthRepo
{
    public class AuthRepository : IAuthRepository
    {
        private readonly EasyWayRideDataContext _context;

        public AuthRepository(EasyWayRideDataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponseData<Driver?>> LoginDriver(string phoneNumber)
        {
            var driver = await _context.Drivers.Where(x => x.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
            if (driver == null)
            {
                return new ServiceResponseData<Driver?>()
                {
                    Message = "Error"
                };
            }
            return new ServiceResponseData<Driver?>()
            {
                Message = "authenticated",
                Data = driver,
                Success = true
            };
        }

        public async Task<ServiceResponseData<Passenger?>> LoginPassenger(string phoneNumber)
        {
            var passenger = await _context.Passengers.Where(x => x.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
            if (passenger == null)
            {
                return new ServiceResponseData<Passenger?>()
                {
                    Message = "Error"
                };
            }
            return new ServiceResponseData<Passenger?>()
            {
                Message = "authenticated",
                Data = passenger,
                Success = true
            };
        }
    }
}
