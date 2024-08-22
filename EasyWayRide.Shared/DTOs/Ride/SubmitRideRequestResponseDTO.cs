namespace EasyWayRide.Shared.DTOs.Ride
{
    public class SubmitRideRequestResponseDTO
    {
        public Guid RequestID { get; set; }
        #region Location
        public string StartLocationName { get; set; } = string.Empty;
        public decimal StartLatitude { get; set; } = decimal.Zero;
        public decimal StartLongitude { get; set; } = decimal.Zero;

        #region Destination Location
        public string DestinationName { get; set; } = string.Empty;
        public decimal DestinationLatitude { get; set; } = decimal.Zero;
        public decimal DestinationLongitude { get; set; } = decimal.Zero;
        #endregion
        #endregion
    }
}
