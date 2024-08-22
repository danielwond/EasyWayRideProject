using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.DataAccess.Data.Entities.dbo
{
    public class Location
    {
        public Guid ID { get; set; }

        public Guid RequestID { get; set; }

        #region Start Location
        public string StartLocationName { get; set; } = string.Empty;
        public string StartLocationCoordinates { get; set; } = string.Empty;
        public decimal StartLatitude { get; set; } = decimal.Zero;
        public decimal StartLongitude { get; set; } = decimal.Zero;
        #endregion

        #region End Location
        public string EndLocationName { get; set; } = string.Empty;
        public string EndLocationCoordinates { get; set; } = string.Empty;
        public decimal EndLatitude { get; set; } = decimal.Zero;
        public decimal EndLongitude { get; set; } = decimal.Zero;
        #endregion

        #region Destination Location
        public string DestinationName { get; set; } = string.Empty;
        public string DestinationCoordinates { get; set; } = string.Empty;
        public decimal DestinationLatitude { get; set; } = decimal.Zero;
        public decimal DestinationLongitude { get; set; } = decimal.Zero;
        #endregion
    }
}
