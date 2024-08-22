using EasyWayRide.DataAccess.Repos.ISeedRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EasyWayRide.Services.Configurations.ExtentionMethods.App
{
    public static class SeedingConfigurations
    {
        public static async Task ConfigureSeedingAsync(this HttpContext context)
        {
            var seedingService = context.RequestServices.GetService<ISeedRepository>();
            if (seedingService != null)
            {
                await seedingService.SeedConfigurationData();
                await seedingService.SeedCarTypes();
            }
        }
    }
}
