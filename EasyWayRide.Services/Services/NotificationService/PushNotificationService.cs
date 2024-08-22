using FirebaseAdmin.Messaging;


namespace EasyWayRide.Services.Services.NotificationService
{
    public class PushNotificationService : IPushNotificationService
    {

        public async Task<string> SendFcmMessageAsync(string token, string title, string body)
        {
            try
            {
                var message = new Message()
                {
                    Token = token,
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body
                    }
                };
                var ss = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return ss;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
