using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.TourDtos;
using Project3Vitour.Services.TourServices;

namespace Project3Vitour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminTourController : Controller
    {
        private readonly ITourService _tourService;

        public AdminTourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        public async Task<IActionResult> Index(int page = 1, string filter = "all", string search = "")
        {
            const int pageSize = 10;
            ViewBag.Title = "Turlar";
            ViewBag.ActiveMenu = "tours";

            var allTours = await _tourService.GetAllTourAsync();

            ViewBag.TotalCount = allTours.Count;
            ViewBag.ActiveCount = allTours.Count(x => x.Status);
            ViewBag.PassiveCount = allTours.Count(x => !x.Status);
            ViewBag.CurrentSearch = search;

            var filtered = filter switch
            {
                "active" => allTours.Where(x => x.Status).ToList(),
                "passive" => allTours.Where(x => !x.Status).ToList(),
                _ => allTours
            };

            if (!string.IsNullOrEmpty(search))
                filtered = filtered.Where(x => x.Title != null &&
                    x.Title.ToLower().Contains(search.ToLower())).ToList();

            var paged = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(filtered.Count / (double)pageSize);
            ViewBag.CurrentFilter = filter;

            return View(paged);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Yeni Tur Ekle";
            ViewBag.ActiveMenu = "tours";
            return View(new CreateTourDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTourDto dto,
            string IncludedServicesText, string ExcludedServicesText,
            string AmenitiesText, string GalleryImagesText, string ExpectListText,
            List<string> HighlightsText)
        {
            dto.IncludedServices = ParseLines(IncludedServicesText);
            dto.ExcludedServices = ParseLines(ExcludedServicesText);
            dto.Amenities = ParseLines(AmenitiesText);
            dto.GalleryImages = ParseLines(GalleryImagesText);
            dto.ExpectList = ParseLines(ExpectListText);

            if (dto.DayPlans != null)
            {
                for (int i = 0; i < dto.DayPlans.Count; i++)
                {
                    dto.DayPlans[i].Highlights = HighlightsText != null && i < HighlightsText.Count
                        ? ParseComma(HighlightsText[i])
                        : new();
                }
            }

            await _tourService.CreateTourAsync(dto);
            TempData["Success"] = "Tur başarıyla eklendi!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            ViewBag.Title = "Turu Düzenle";
            ViewBag.ActiveMenu = "tours";
            ViewBag.TourID = id;

            var tour = await _tourService.GetTourByIdAsync(id);

            var dto = new UpdateTourDto
            {
                TourID = tour.TourID,
                Title = tour.Title,
                Description = tour.Description,
                CoverImageUrl = tour.CoverImageUrl,
                Badge = tour.Badge,
                DayCount = tour.DayCount,
                Capacity = tour.Capacity,
                Price = tour.Price,
                Status = tour.Status,
                Location = tour.Location,
                DepartureLocation = tour.DepartureLocation,
                DepartureTime = tour.DepartureTime,
                ReturnTime = tour.ReturnTime,
                AdvanceFacilities = tour.AdvanceFacilities,
                WhatToExpect = tour.WhatToExpect,
                Latitude = tour.Latitude,
                Longitude = tour.Longitude,
                IncludedServices = tour.IncludedServices ?? new(),
                ExcludedServices = tour.ExcludedServices ?? new(),
                Amenities = tour.Amenities ?? new(),
                GalleryImages = tour.GalleryImages ?? new(),
                ExpectList = tour.ExpectList ?? new(),
                DayPlans = tour.DayPlans?.Select(d => new TourDayPlanDto
                {
                    DayNumber = d.DayNumber,
                    Title = d.Title,
                    Description = d.Description,
                    Highlights = d.Highlights ?? new()
                }).ToList() ?? new()
            };

            ViewBag.IncludedServicesText = string.Join("\n", dto.IncludedServices);
            ViewBag.ExcludedServicesText = string.Join("\n", dto.ExcludedServices);
            ViewBag.AmenitiesText = string.Join("\n", dto.Amenities);
            ViewBag.GalleryImagesText = string.Join("\n", dto.GalleryImages);
            ViewBag.ExpectListText = string.Join("\n", dto.ExpectList);

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateTourDto dto,
            string IncludedServicesText, string ExcludedServicesText,
            string AmenitiesText, string GalleryImagesText, string ExpectListText,
            List<string> HighlightsText)
        {
            dto.IncludedServices = ParseLines(IncludedServicesText);
            dto.ExcludedServices = ParseLines(ExcludedServicesText);
            dto.Amenities = ParseLines(AmenitiesText);
            dto.GalleryImages = ParseLines(GalleryImagesText);
            dto.ExpectList = ParseLines(ExpectListText);

            if (dto.DayPlans != null)
            {
                for (int i = 0; i < dto.DayPlans.Count; i++)
                {
                    dto.DayPlans[i].Highlights = HighlightsText != null && i < HighlightsText.Count
                        ? ParseComma(HighlightsText[i])
                        : new();
                }
            }

            await _tourService.UpdateTourAsync(dto);
            TempData["Success"] = "Tur başarıyla güncellendi!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _tourService.DeleteTourAsync(id);
            TempData["Success"] = "Tur başarıyla silindi!";
            return RedirectToAction("Index");
        }

        private List<string> ParseLines(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new();
            return text.Split('\n')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();
        }

        private List<string> ParseComma(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new();
            return text.Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();
        }
    }
}