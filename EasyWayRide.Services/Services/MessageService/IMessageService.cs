namespace EasyWayRide.Services.Services.MessageService
{
    public interface IMessageService
    {
        public void PublishMessage(string message, string driverId);
    }
}
