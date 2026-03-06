using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Services.GuideServices;

namespace Project3Vitour.Controllers
{
    public class GuideController : Controller
    {
        private readonly IGuideService _guideService;

        public GuideController(IGuideService guideService)
        {
            _guideService = guideService;
        }

        public IActionResult Index()
        {
            ViewBag.BreadcrumbTitle = "Rehberlerimiz";
            ViewBag.BreadcrumbParent = "Anasayfa";
            ViewBag.BreadcrumbParentUrl = "/";
            return View();
        }
    }
}