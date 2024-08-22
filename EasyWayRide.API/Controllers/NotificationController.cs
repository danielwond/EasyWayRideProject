using EasyWayRide.Services.Services.NotificationService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyWayRide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IPushNotificationService _pushNotification;

        public NotificationController(IPushNotificationService pushNotification)
        {
            _pushNotification = pushNotification;
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification(string title, string body)
        {
            var token = "dbgVR8sXTRW2Fg4w7DNDMA:APA91bHOyj_ZO6e139LCP0xWLKMzgihsZwJC2fjsccjJ8-IjgCxDRJvH4gb_iI-zE3TcT8kf6UcBMLVw5T3a8O4mZN6yopPR_ShHYXIZGuTek6OKs0-S_4jlrv7Z7xEJ1lBNjOBVH8XK";
            var result = await _pushNotification.SendFcmMessageAsync(token, title, body);
            return Ok(result);
        }
    }
}
