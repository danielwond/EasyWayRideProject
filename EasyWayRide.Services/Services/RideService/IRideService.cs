using EasyWayRide.Shared.DTOs.Ride;
using EasyWayRide.Shared.Responses;

namespace EasyWayRide.Services.Services.RideService
{
    public interface IRideService
    {
        public Task<ServiceResponseData<SubmitRideRequestResponseDTO>> SubmitRideRequest(CreateRideRequestDTO ride, Guid PassengerID, Guid CarTypeID);
        public Task<ServiceResponseData<AcceptRideResponseDTO>> AcceptRide(Guid requestID, Guid driverID);
        public Task<ServiceResponseMessage> StartRide(Guid requestID, string StartLocationName, string StartLocationCoordinates, decimal StartLatitude, decimal StartLongitude);
        public Task<ServiceResponseData<EndRideResponseDTO>> EndRide(Guid requestID, string EndLocationName, string EndLocationCoordinates, decimal TripEndLatitude, decimal TripEndLongitude);
        public Task<ServiceResponseMessage> CancelRide(Guid requestID, string? cancellationReason);
    }
}
