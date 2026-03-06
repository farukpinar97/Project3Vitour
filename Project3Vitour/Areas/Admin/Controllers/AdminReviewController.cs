using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Services.TourServices;

namespace Project3Vitour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminReviewController : Controller
    {
        private readonly ITourReviewService _reviewService;
        private readonly ITourService _tourService;

        public AdminReviewController(ITourReviewService reviewService, ITourService tourService)
        {
            _reviewService = reviewService;
            _tourService = tourService;
        }

        public async Task<IActionResult> Index(int page = 1, string filter = "all")
        {
            const int pageSize = 10;

            var allReviews = await _reviewService.GetAllReviewsAsync();
            var allTours = await _tourService.GetAllTourAsync();

            ViewBag.TotalCount = allReviews.Count;
            ViewBag.ApprovedCount = allReviews.Count(x => x.IsApproved);
            ViewBag.PendingCount = allReviews.Count(x => !x.IsApproved);
            ViewBag.AverageRating = allReviews.Any()
                ? allReviews.Average(r => (r.ValueRating + r.DestinationRating + r.AccommodationRating + r.TransportRating) / 4.0)
                : 0;

            ViewBag.AvgValue = allReviews.Any() ? allReviews.Average(r => (double)r.ValueRating) : 0;
            ViewBag.AvgDestination = allReviews.Any() ? allReviews.Average(r => (double)r.DestinationRating) : 0;
            ViewBag.AvgAccommodation = allReviews.Any() ? allReviews.Average(r => (double)r.AccommodationRating) : 0;
            ViewBag.AvgTransport = allReviews.Any() ? allReviews.Average(r => (double)r.TransportRating) : 0;

            var tourStats = allReviews
                .GroupBy(r => r.TourID)
                .Select(g => new
                {
                    TourID = g.Key,
                    TourTitle = allTours.FirstOrDefault(t => t.TourID == g.Key)?.Title ?? "Bilinmeyen Tur",
                    ReviewCount = g.Count(),
                    AvgRating = g.Average(r => (r.ValueRating + r.DestinationRating + r.AccommodationRating + r.TransportRating) / 4.0)
                })
                .OrderByDescending(x => x.AvgRating)
                .ToList();

            ViewBag.TopTours = tourStats.Take(3).ToList();
            ViewBag.BottomTours = tourStats.TakeLast(3).ToList();

            // Filtre uygula
            var filtered = filter switch
            {
                "approved" => allReviews.Where(x => x.IsApproved).ToList(),
                "pending" => allReviews.Where(x => !x.IsApproved).ToList(),
                _ => allReviews
            };

            int totalFiltered = filtered.Count;
            var paged = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalFiltered / (double)pageSize);
            ViewBag.CurrentFilter = filter;

            return View(paged);
        }

        public async Task<IActionResult> Approve(string id)
        {
            await _reviewService.ApproveReviewAsync(id);
            TempData["Success"] = "Yorum onaylandı!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _reviewService.DeleteReviewAsync(id);
            TempData["Success"] = "Yorum silindi!";
            return RedirectToAction("Index");
        }
    }
}