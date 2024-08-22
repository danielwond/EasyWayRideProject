using AutoMapper;
using EasyWayRide.DataAccess.Data.Entities.configurations;
using EasyWayRide.DataAccess.Repos.IConfigRepo;
using EasyWayRide.Shared.DTOs.IConfig;
using EasyWayRide.Shared.DTOs.Vehicle;
using EasyWayRide.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Services.Services.ConfigService
{
    public class ConfigService : IConfigService
    {
        private readonly IConfigRepository _configRepository;
        private readonly IMapper _mapper;

        public ConfigService(IConfigRepository configRepository, IMapper mapper)
        {
            _configRepository = configRepository;
            _mapper = mapper;
        }

        public Task<ServiceResponseData<List<CarType>>> GetCarTypes()
        {
            return _configRepository.GetAllCarTypes();
        }

        public async Task<ServiceResponseMessage> InsertCarType(AddCarTypeRequestDto carTypeRequest)
        {
            var carType = _mapper.Map<CarType>(carTypeRequest);

            carType.IsActive = false;
            var result = await _configRepository.InsertCarType(carType);
            return result;
        }
    }
}
