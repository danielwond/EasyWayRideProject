using AutoMapper;
using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.Shared.DTOs.Passenger;

namespace EasyWayRide.Services.Mappings
{
    public class PassengerMappings : Profile
    {
        public PassengerMappings()
        {
            CreateMap<RegisterPassengerDTO, Passenger>().ReverseMap();
        }
    }
}
