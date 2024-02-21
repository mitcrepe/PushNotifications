using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebClient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Notifications;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly NotificationService _notificationService;

        public HomeController(IConfiguration config, NotificationService notificationService)
        {
            _config = config;
            _notificationService = notificationService;
        }

        public IActionResult Dashboard()
        {
            DashboardModel model = new()
            {
                PublicKey = _config["VAPID:PublicKey"],
                AvailableUsers = _notificationService.GetAllSubscribedUsers()
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
