using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Areas.Api.Models;

namespace WebClient.Api.Controllers
{
    [Area("Api")]
    [Route("api/[controller]")]
    [ApiController]
    public class PushController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public PushController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        [HttpPost("Subscribe")]
        public void Subscribe(Subscription subscription)
        {
            if (subscription is null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            if (string.IsNullOrEmpty(subscription.Username))
            {
                throw new ArgumentNullException(nameof(subscription.Username));
            }

            _notificationService.Subscribe(subscription);
        }

        [HttpPost("Unsubscribe")]
        public void Unsubscribe(string username)
        {
            _notificationService.Unsubscribe(username);
        }

        [HttpPost("SendMessageToUser")]
        public async Task<IActionResult> SendMessageToUser([FromForm] string targetUser, [FromForm] Message message)
        {
            if (string.IsNullOrEmpty(targetUser))
            {
                return BadRequest($"'{nameof(targetUser)}' cannot be null or empty.");
            }

            await _notificationService.NotifyUser(targetUser, message);
            return Ok();
        }
    }
}
