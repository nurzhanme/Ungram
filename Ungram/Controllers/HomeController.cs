using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Ungram.Models;
using Ungram.Services;

namespace Ungram.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly InstaService _instaService;
        public HomeController(ILogger<HomeController> logger, InstaService instaService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _instaService = instaService ?? throw new ArgumentNullException(nameof(instaService));
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
                var authResult = await _instaService.Login(vm.Username, vm.Password);

                if (authResult.Item1)
                {
                    ViewData["login"] = true;
                }

                var result = await _instaService.GetNotFollowingBack(vm.Username);
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