using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Services.BookingServices;
using Project3Vitour.Services.ContactServices;
using Project3Vitour.Services.GuideServices;
using Project3Vitour.Services.TourServices;

namespace Project3Vitour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IBookingService _bookingService;
        private readonly ITourReviewService _reviewService;
        private readonly IContactService _contactService;

        public AdminController(
            ITourService tourService,
            IBookingService bookingService,
            ITourReviewService reviewService,
            IContactService contactService)
        {
            _tourService = tourService;
            _bookingService = bookingService;
            _reviewService = reviewService;
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Kontrol Paneli";
            ViewBag.ActiveMenu = "dashboard";

            // ── İstatistikler ──────────────────────────────
            var tours = await _tourService.GetAllTourAsync();
            var bookings = await _bookingService.GetAllBookingsAsync();
            var reviews = await _reviewService.GetAllReviewsAsync();
            var messages = await _contactService.GetAllMessagesAsync();

            ViewBag.TotalTours = tours.Count;
            ViewBag.ActiveTours = tours.Count(x => x.Status);

            ViewBag.TotalReservations = bookings.Count;
            ViewBag.PendingReservations = bookings.Count(x => x.Status == "Pending");
            ViewBag.ConfirmedReservations = bookings.Count(x => x.Status == "Confirmed");
            ViewBag.TotalRevenue = bookings
                .Where(x => x.Status == "Confirmed")
                .Sum(x => x.TotalPrice);

            ViewBag.TotalReviews = reviews.Count;
            ViewBag.PendingReviews = reviews.Count(x => !x.IsApproved);
            ViewBag.ApprovedReviews = reviews.Count(x => x.IsApproved);
            ViewBag.AverageRating = reviews.Any()
                ? reviews.Average(r => (r.ValueRating + r.DestinationRating + r.AccommodationRating + r.TransportRating) / 4.0)
                : 0;

            ViewBag.TotalMessages = messages.Count;
            ViewBag.UnreadMessages = messages.Count(x => !x.IsRead);

            // ── Son Veriler ────────────────────────────────
            ViewBag.RecentReservations = bookings
                .OrderByDescending(x => x.BookingDate)
                .Take(5)
                .ToList();

            ViewBag.RecentMessages = messages
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .ToList();

            ViewBag.RecentReviews = reviews
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .ToList();

            return View();
        }
    }
}