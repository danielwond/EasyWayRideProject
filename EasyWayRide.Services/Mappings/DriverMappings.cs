using AutoMapper;
using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.Shared.DTOs.Driver;

namespace EasyWayRide.Services.Mappings
{
    public class DriverMappings : Profile
    {
        public DriverMappings()
        {
            CreateMap<RegisterDriverRequestDTO, Driver>().ReverseMap();
        }
    }
}
