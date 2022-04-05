using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using InstagramUnfollowers;
using Ungram.Models;

namespace Ungram.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Client _client;
        public HomeController(ILogger<HomeController> logger, Client client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Username,Password,SaveSession")] LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var authResult = await _client.Login(vm.Username, vm.Password);

                if (!string.IsNullOrWhiteSpace(authResult))
                {
                    ViewData["login"] = true;
                }

                var result = await _client.GetNotFollowingBack(vm.Username);
                ViewData["NotFollowingBack"] = result;
                return View();
            }
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}