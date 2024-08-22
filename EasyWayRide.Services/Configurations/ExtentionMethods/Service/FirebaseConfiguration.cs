using Microsoft.Extensions.DependencyInjection;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;


namespace EasyWayRide.Services.Configurations.ExtentionMethods.Service
{
    public static class FirebaseConfiguration
    {
        public static void ConfigureFirebase(this IServiceCollection services)
        {
            // Add FirebaseApp initialization code here
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("firebase.json")
            });
        }
    }
}
