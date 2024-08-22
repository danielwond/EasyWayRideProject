using EasyWayRide.Services.Services.EnvironmentService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyWayRide.Services.Configurations.ExtentionMethods.App
{
    public static class EnvironmentConfigurations
    {
        public static void ConfigureEnvironment(this HttpContext context)
        {
            var config = context.RequestServices.GetService<IConfiguration>();
            var isDev = bool.Parse(config["Environment:isDev"]);

            var environmentService = context.RequestServices.GetService<IEnvironmentService>();
            if (environmentService != null)
            {
                environmentService.isDevelopment = isDev;
            }

            if (!File.Exists("appsettings.json"))
            {
                throw (new FileNotFoundException("appsettings.json is not found"));
            }
        }
    }
}
