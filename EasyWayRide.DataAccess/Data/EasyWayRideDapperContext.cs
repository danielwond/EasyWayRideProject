using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.DataAccess.Data
{
    public class EasyWayRideDapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public EasyWayRideDapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("EasyWayRideAPIContextConnection")!;
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
