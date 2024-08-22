using EasyWayRide.Services.Mappings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Services.Configurations.ExtentionMethods.Service
{
    public static class MappingsConfiguration
    {
        public static IServiceCollection ConfigureMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DriverMappings));
            services.AddAutoMapper(typeof(PassengerMappings));
            services.AddAutoMapper(typeof(VehicleMappings));
            services.AddAutoMapper(typeof(RideRequestMappings));

            return services;
        }
    }
}
