using EasyWayRide.DataAccess.Hubs;
using Microsoft.AspNetCore.Builder;

namespace EasyWayRide.Services.Configurations.ExtentionMethods.App
{
    public static class HubUsings
    {
        public static void UseHubs(this WebApplication app)
        {
            app.MapHub<DriverHub>("/onlinedrivers");
        }
    }
}
