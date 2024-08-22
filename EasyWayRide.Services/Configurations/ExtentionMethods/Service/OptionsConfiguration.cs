using EasyWayRide.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyWayRide.Services.Configurations.ExtentionMethods.Service
{
    public static class OptionsConfiguration
    {
        public static IServiceCollection ConfigureOptions(this IServiceCollection services)
        {
            var conf = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            services.Configure<JwtOptions>(conf.GetSection("JWT"));
            services.Configure<GoogleOptions>(conf.GetSection("Google"));
            services.Configure<EncryptionOptions>(conf.GetSection("Encryption"));

            return services;
        }
    }
}
