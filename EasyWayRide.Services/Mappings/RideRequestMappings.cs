using AutoMapper;
using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.Shared.DTOs.Ride;

namespace EasyWayRide.Services.Mappings
{
    public class RideRequestMappings : Profile
    {
        public RideRequestMappings()
        {
            CreateMap<CreateRideRequestDTO, RideRequest>().ReverseMap();
        }
    }
}
