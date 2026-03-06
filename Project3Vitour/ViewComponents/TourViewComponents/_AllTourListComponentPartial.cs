using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Services.TourServices;

namespace Project3Vitour.ViewComponents.TourViewComponents
{
    public class _AllTourListComponentPartial : ViewComponent
    {
        private readonly ITourService _tourService;
        private readonly ITourReviewService _reviewService;
        private const int PageSize = 6;

        public _AllTourListComponentPartial(ITourService tourService, ITourReviewService reviewService)
        {
            _tourService = tourService;
            _reviewService = reviewService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            int page = 1,
            string sort = "",
            string location = "",
            int minPrice = 0,
            int maxPrice = 0,
            int minDay = 0,
            int maxDay = 0,
            int guests = 0)
        {
            if (page < 1) page = 1;

            var allTours = await _tourService.GetAllTourAsync();

            // Lokasyon filtresi
            if (!string.IsNullOrEmpty(location))
                allTours = allTours.Where(x =>
                    x.Location != null &&
                    x.Location.ToLower().Contains(location.ToLower())).ToList();

            // Fiyat filtresi
            if (minPrice > 0)
                allTours = allTours.Where(x => x.Price >= minPrice).ToList();
            if (maxPrice > 0)
                allTours = allTours.Where(x => x.Price <= maxPrice).ToList();

            // Süre filtresi
            if (minDay > 0)
                allTours = allTours.Where(x => x.DayCount >= minDay).ToList();
            if (maxDay > 0)
                allTours = allTours.Where(x => x.DayCount <= maxDay).ToList();

            // Kişi sayısı filtresi
            if (guests > 0)
                allTours = allTours.Where(x => x.Capacity >= guests).ToList();

            // Sıralama
            allTours = sort switch
            {
                "price_asc" => allTours.OrderBy(x => x.Price).ToList(),
                "price_desc" => allTours.OrderByDescending(x => x.Price).ToList(),
                "new" => allTours.OrderByDescending(x => x.TourID).ToList(),
                _ => allTours
            };

            long totalCount = allTours.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
            if (page > totalPages && totalPages > 0) page = totalPages;

            var pagedTours = allTours.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            foreach (var tour in pagedTours)
            {
                var reviews = await _reviewService.GetReviewsByTourIdAsync(tour.TourID);
                tour.ReviewCount = reviews.Count;
                tour.AverageRating = reviews.Count > 0
                    ? reviews.Average(r => (r.ValueRating + r.DestinationRating + r.AccommodationRating + r.TransportRating) / 4.0)
                    : 0;
            }

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.PageSize = PageSize;
            ViewBag.CurrentSort = sort;
            ViewBag.CurrentLocation = location;
            ViewBag.CurrentMinPrice = minPrice;
            ViewBag.CurrentMaxPrice = maxPrice;
            ViewBag.CurrentMinDay = minDay;
            ViewBag.CurrentMaxDay = maxDay;
            ViewBag.CurrentGuests = guests;

            return View(pagedTours);
        }
    }
}