using EasyWayRide.DataAccess.Hubs;
using EasyWayRide.Services.Configurations.ExtentionMethods.App;
using EasyWayRide.Services.Configurations.ExtentionMethods.Service;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Logging Section
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/error_log.log", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {CorrelationId} {Level:u3} {Username} {Message:lj}{Exception}{ExceptionStackTrace}{NewLine}")
    .CreateLogger();
#endregion

var appsettings = builder.Configuration[""]

builder.Services.ConfigureDbContext();
builder.Services.ConfigureIdentity();

// Add services to the container.
builder.Services
    .ConfigureRepositoryInjections()
    .ConfigureServiceInjections()
    .ConfigureMappings()
    .ConfigureHubs()
    .ConfigureOptions()
    .ConfigureFirebase();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseResponseCompression();

app.Use(async (context, next) =>
{
    context.ConfigureEnvironment();
    await context.ConfigureSeedingAsync();

    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
    app.UseCors(s => s.AllowCredentials().AllowAnyHeader());
    app.MapHub<DriverHub>("/onlinedrivers");

    app.Run();
}
else
{
    app.UsePathBase("/ride");
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
    app.UseCors(s => s.AllowCredentials().AllowAnyHeader());
    app.MapHub<DriverHub>("/onlinedrivers");

    app.Run("http://127.0.0.1:8080");

}

