using AutoMapper;
using EasyWayRide.DataAccess.Data.Entities.configurations;
using EasyWayRide.Shared.DTOs.IConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Services.Mappings
{
    public class ConfigMappings : Profile
    {
        public ConfigMappings()
        {
            CreateMap<AddCarTypeRequestDto, CarType>().ReverseMap();
        }
    }
}
