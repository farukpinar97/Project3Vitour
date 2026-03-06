using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Services.TourServices;
using Project3Vitour.Dtos.ReviewDtos;

namespace Project3Vitour.ViewComponents.TourViewComponents
{
    public class _TourDetailComponentPartial : ViewComponent
    {
        private readonly ITourService _tourService;
        private readonly ITourReviewService _reviewService;

        public _TourDetailComponentPartial(ITourService tourService, ITourReviewService reviewService)
        {
            _tourService = tourService;
            _reviewService = reviewService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            tour.Reviews = await _reviewService.GetReviewsByTourIdAsync(id);
            ViewBag.AverageRating = await _reviewService.GetAverageRatingAsync(id);
            return View(tour);
        }
    }
}