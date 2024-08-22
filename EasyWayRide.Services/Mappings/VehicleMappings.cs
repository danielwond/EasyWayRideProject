using AutoMapper;
using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.Shared.DTOs.Vehicle;

namespace EasyWayRide.Services.Mappings
{
    public class VehicleMappings : Profile
    {
        public VehicleMappings()
        {
            CreateMap<RegisterVehicleDTO, Vehicle>().ForMember(x => x.CarType, opt => opt.Ignore()).ReverseMap();
            CreateMap<GetVehicleTypesDTO, Vehicle>()
                .ForMember(dest => dest.ID, map => map.MapFrom(x => x.CarTypeID))
                .ForPath(dest => dest.CarType!.PerKmRate, map => map.MapFrom(x => x.Price))
                .ForMember(dest => dest.PlateNumber, map => map.MapFrom(x => x.Name)).ReverseMap();

        }
    }
}
