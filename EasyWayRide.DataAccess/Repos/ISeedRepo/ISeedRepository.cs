namespace EasyWayRide.DataAccess.Repos.ISeedRepo
{
    public interface ISeedRepository
    {
        Task SeedConfigurationData();
        Task SeedCarTypes();
    }
}
