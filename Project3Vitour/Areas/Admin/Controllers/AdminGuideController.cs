using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.GuideDtos;
using Project3Vitour.Services.GuideServices;

namespace Project3Vitour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminGuideController : Controller
    {
        private readonly IGuideService _guideService;

        public AdminGuideController(IGuideService guideService)
        {
            _guideService = guideService;
        }

        public async Task<IActionResult> Index(int page = 1, string filter = "all")
        {
            const int pageSize = 10;
            ViewBag.Title = "Rehberler";
            ViewBag.ActiveMenu = "guides";

            var allGuides = await _guideService.GetAllGuidesForAdminAsync();

            ViewBag.TotalCount = allGuides.Count;
            ViewBag.ActiveCount = allGuides.Count(x => x.Status);
            ViewBag.PassiveCount = allGuides.Count(x => !x.Status);

            var filtered = filter switch
            {
                "active" => allGuides.Where(x => x.Status).ToList(),
                "passive" => allGuides.Where(x => !x.Status).ToList(),
                _ => allGuides
            };

            int totalFiltered = filtered.Count;
            var paged = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalFiltered / (double)pageSize);
            ViewBag.CurrentFilter = filter;

            return View(paged);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Yeni Rehber Ekle";
            ViewBag.ActiveMenu = "guides";
            return View(new CreateGuideDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGuideDto dto)
        {
            await _guideService.CreateGuideAsync(dto);
            TempData["Success"] = "Rehber başarıyla eklendi!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            ViewBag.Title = "Rehberi Düzenle";
            ViewBag.ActiveMenu = "guides";
            var guide = await _guideService.GetGuideByIdAsync(id);
            var dto = new UpdateGuideDto
            {
                GuideID = guide.GuideID,
                FullName = guide.FullName,
                Title = guide.Title,
                ImageUrl = guide.ImageUrl,
                Facebook = guide.Facebook,
                Instagram = guide.Instagram,
                LinkedIn = guide.LinkedIn,
                YouTube = guide.YouTube,
                Status = guide.Status
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateGuideDto dto)
        {
            await _guideService.UpdateGuideAsync(dto);
            TempData["Success"] = "Rehber başarıyla güncellendi!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _guideService.DeleteGuideAsync(id);
            TempData["Success"] = "Rehber başarıyla silindi!";
            return RedirectToAction("Index");
        }
    }
}