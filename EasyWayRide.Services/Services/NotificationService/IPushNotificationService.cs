using FirebaseAdmin.Messaging;

namespace EasyWayRide.Services.Services.NotificationService
{
    public interface IPushNotificationService
    {
        public Task<string> SendFcmMessageAsync(string token, string title, string body);
    }
}
