using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWayRide.Services.Configurations.ExtentionMethods.Service
{
    public static class HubsConfiguration
    {
        public static IServiceCollection ConfigureHubs(this IServiceCollection services)
        {
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
            });
            services.AddSignalR(config =>
            {
                config.EnableDetailedErrors = true;
            });
            return services;
        }
    }
}
