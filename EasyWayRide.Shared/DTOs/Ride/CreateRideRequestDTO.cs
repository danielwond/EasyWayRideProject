namespace EasyWayRide.Shared.DTOs.Ride
{
    public class CreateRideRequestDTO
    {
        #region Location
        public string StartLocationName { get; set; } = string.Empty;
        public string StartLocationCoordinates { get; set; } = string.Empty;
        public decimal StartLatitude { get; set; } = decimal.Zero;
        public decimal StartLongitude { get; set; } = decimal.Zero;

        #region Destination Location
        public string DestinationName { get; set; } = string.Empty;
        public string DestinationCoordinates { get; set; } = string.Empty;
        public decimal DestinationLatitude { get; set; } = decimal.Zero;
        public decimal DestinationLongitude { get; set; } = decimal.Zero;
        #endregion
        #endregion

        #region Fare
        public decimal EstimatedFare { get; set; } = 0;
        #endregion
    }
}
