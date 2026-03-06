using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Models;
using Project3Vitour.Services.TourServices;
using System.Diagnostics;

namespace Project3Vitour.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITourService _tourService;

        public HomeController(ILogger<HomeController> logger, ITourService tourService)
        {
            _logger = logger;
            _tourService = tourService;
        }

        public async Task<IActionResult> Index()
        {
            var featuredTours = await _tourService.GetFeaturedToursAsync(5);
            return View(featuredTours);
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
