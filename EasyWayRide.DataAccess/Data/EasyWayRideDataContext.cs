using EasyWayRide.DataAccess.Data.Entities.configurations;
using EasyWayRide.DataAccess.Data.Entities.dbo;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EasyWayRide.DataAccess.Data;

public class EasyWayRideDataContext : IdentityDbContext<Employee>
{
    public EasyWayRideDataContext(DbContextOptions<EasyWayRideDataContext> options)
        : base(options)
    {
    }

    #region DBO
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<RideRequest> RideRequests { get; set; }
    public DbSet<Location> Locations { get; set; }

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<FareData> FareData { get; set; }
    #endregion
    #region Configuration
    public DbSet<RideStatus> RideStatus { get; set; }
    public DbSet<CarType> CarTypes { get; set; }
    public DbSet<ConfigValue> ConfigValues { get; set; }

    #endregion



    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.Entity<OnlineDriver>().ToTable(tb => tb.HasTrigger("TriggerName"));


        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
