using EasyWayRide.DataAccess.Data;
using EasyWayRide.DataAccess.Data.Entities.dbo;
using EasyWayRide.DataAccess.Repos.IAuthRepo;
using EasyWayRide.DataAccess.Repos.ICachingRepo;
using EasyWayRide.DataAccess.Repos.IConfigRepo;
using EasyWayRide.DataAccess.Repos.IDriverRepo;
using EasyWayRide.DataAccess.Repos.IPassengerRepo;
using EasyWayRide.DataAccess.Repos.IRideRepo;
using EasyWayRide.DataAccess.Repos.ISeedRepo;
using EasyWayRide.DataAccess.Repos.IVehicleRepo;
using EasyWayRide.Services.Services.AuthService;
using EasyWayRide.Services.Services.ConfigService;
using EasyWayRide.Services.Services.DriverService;
using EasyWayRide.Services.Services.EncryptionService;
using EasyWayRide.Services.Services.EnvironmentService;
using EasyWayRide.Services.Services.MessageService;
using EasyWayRide.Services.Services.NotificationService;
using EasyWayRide.Services.Services.PassengerService;
using EasyWayRide.Services.Services.RideService;
using EasyWayRide.Services.Services.TokenService;
using EasyWayRide.Services.Services.VehicleService;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyWayRide.Services.Configurations.ExtentionMethods.Service
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection ConfigureRepositoryInjections(this IServiceCollection services)
        {
            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddScoped<IRideRepository, RideRepository>();
            services.AddScoped<IPassengerRepository, PassengerRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IConfigRepository, ConfigRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ICachingRepository, CachingRepository>();

            services.AddTransient<EasyWayRideDapperContext>();

            return services;
        }
        public static IServiceCollection ConfigureServiceInjections(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<IEnvironmentService, EnvironmentService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPassengerService, PassengerService>();
            services.AddScoped<IRideService, RideService>();
            services.AddScoped<IConfigService, ConfigService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<IPushNotificationService, PushNotificationService>();
            services.AddScoped<ISeedRepository, SeedRepository>();

            services.AddSingleton<IMessageService, MessageService>();

            return services;

        }
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<Employee>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<EasyWayRideDataContext>();
            return services;
        }
        public static IServiceCollection ConfigureDbContext(this IServiceCollection services)
        {
            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            var connectionString = configuration.GetConnectionString("EasyWayRideAPIContextConnection") ?? throw new InvalidOperationException("Connection string 'EasyWayRideAPIContextConnection' not found.");

            services.AddDbContext<EasyWayRideDataContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
        public static IServiceCollection ConfigureRedis(this IServiceCollection services)
        {
            return services;
        }
    }
}
