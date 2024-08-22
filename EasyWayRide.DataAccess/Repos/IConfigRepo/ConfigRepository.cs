using EasyWayRide.DataAccess.Data;
using EasyWayRide.DataAccess.Data.Entities.configurations;
using EasyWayRide.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace EasyWayRide.DataAccess.Repos.IConfigRepo
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly EasyWayRideDataContext _context;

        public ConfigRepository(EasyWayRideDataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponseData<List<CarType>>> GetAllCarTypes()
        {
            return (new ServiceResponseData<List<CarType>>()
            {
                Data = await Task.FromResult(_context.CarTypes.AsQueryable().ToList()),
                Message = "all_fetched",
                Success = true
            });
        }

        public async Task<ServiceResponseData<List<ConfigValue>>> GetAllConfigValues()
        {
            var result = await _context.ConfigValues.AsNoTracking().ToListAsync();
            return new ServiceResponseData<List<ConfigValue>>()
            {
                Data = result,
                Message = "all_fetched",
                Success = true
            };
        }

        public async Task<ServiceResponseMessage> InsertCarType(CarType carTypeModel)
        {
            var carType = await _context.CarTypes.Where(x => x.Name == carTypeModel.Name).FirstOrDefaultAsync();
            if (carType != null)
            {
                return new ServiceResponseMessage()
                {
                    Message = $"{carTypeModel.Name} already registered",
                };
            }
            else
            {
                await _context.AddAsync(carTypeModel);
                await _context.SaveChangesAsync();

                return new ServiceResponseMessage()
                {
                    Message = "all_done",
                    Success = true
                };
            }
        }
    }
}
