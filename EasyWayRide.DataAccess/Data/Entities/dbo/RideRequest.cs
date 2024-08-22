using EasyWayRide.DataAccess.Data.Entities.configurations;
using EasyWayRide.Shared.Enums;

namespace EasyWayRide.DataAccess.Data.Entities.dbo
{
    public class RideRequest
    {
        public Guid ID { get; set; }
        #region Fare
        public decimal TotalFare { get; set; } = 0;
        public decimal EstimatedFare { get; set; } = 0;
        #endregion

        #region Dates
        public DateTime RequestedOnDate { get; set; }
        public DateTime? AcceptedOnDate { get; set; }
        public DateTime? CancelledOnDate { get; set; }
        public DateTime? StartedOnDate { get; set; }
        public DateTime? EndedOnDate { get; set; }
        #endregion

        #region Entities
        public Driver? Driver { get; set; }
        public Passenger Passenger { get; set; }
        public Location? Location { get; set; }
        public CarType CarType { get; set; }
        #endregion

        public RideStatusEnum RideStatus { get; set; }
        public string? CancellationReason { get; set; }
    }
}
