using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.Shared.DTOs.Ride;
using EasyWayRide.Shared.Responses;

namespace EasyWayRide.DataAccess.Repos.IRideRepo
{
    public interface IRideRepository
    {
        public Task<ServiceResponseData<SubmitRideRequestResponseDTO>> SubmitRideRequest(RideRequest ride, Guid PassengerID, Guid CarTypeID);
        public Task<ServiceResponseData<AcceptRideResponseDTO>> AcceptRide(Guid requestID, Guid driverID);
        public Task<ServiceResponseMessage> StartRide(Guid requestID, string StartLocationName, string StartLocationCoordinates, decimal StartLatitude, decimal StartLongitude);
        public Task<ServiceResponseData<EndRideResponseDTO>> EndRide(Guid requestID, string EndLocationName, string EndLocationCoordinates, decimal TripEndLatitude, decimal TripEndLongitude, double distance);
        public Task<ServiceResponseMessage> CancelRide(Guid requestID, string? cancellationReason);
        public Task<ServiceResponseData<RideRequest>> GetRideRequestData(Guid requestID);
    }
}
