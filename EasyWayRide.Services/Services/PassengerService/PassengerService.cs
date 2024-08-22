using AutoMapper;
using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.DataAccess.Repos.IPassengerRepo;
using EasyWayRide.Shared.DTOs.Passenger;
using EasyWayRide.Shared.Enums;
using EasyWayRide.Shared.Responses;

namespace EasyWayRide.Services.Services.PassengerService
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly IMapper _mapper;

        public PassengerService(IPassengerRepository passengerRepository, IMapper mapper)
        {
            _passengerRepository = passengerRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Passenger>> GetAllPassengers()
        {
            return await _passengerRepository.GetAllPassengers();
        }

        public async Task<GetDriversWithInARadiusDto> GetDriversWithinRadiusAsync(double latitude, double longitude)
        {
            var result = await _passengerRepository.GetDriversWithinRadiusAsync(latitude, longitude);
            return result;
        }

        public async Task<ServiceResponseMessage> RegisterPassenger(RegisterPassengerDTO passenger)
        {
            var passenger_model = _mapper.Map<Passenger>(passenger);

            passenger_model.ID = new Guid();
            passenger_model.AccountStatus = AccountStatusEnum.Active;
            passenger_model.FcmToken = passenger.FCMToken;

            var result = await _passengerRepository.RegisterPassenger(passenger_model);
            return result;
        }
    }
}
