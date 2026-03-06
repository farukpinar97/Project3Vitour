using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.BookingDtos;
using Project3Vitour.Services.BookingServices;
using Project3Vitour.Services.TourServices;

namespace Project3Vitour.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ITourService _tourService;

        public BookingController(IBookingService bookingService, ITourService tourService)
        {
            _bookingService = bookingService;
            _tourService = tourService;
        }

        public async Task<IActionResult> Create(string id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null) return NotFound();

            var totalParticipants = await _bookingService.GetTotalParticipantsAsync(id);
            var remaining = tour.Capacity - totalParticipants;

            ViewBag.RemainingCapacity = remaining;
            ViewBag.BreadcrumbTitle = "Rezervasyon Yap";
            ViewBag.BreadcrumbParent = "Turlar";
            ViewBag.BreadcrumbParentUrl = "/Tour/TourList";

            return View(tour);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            var tour = await _tourService.GetTourByIdAsync(dto.TourID);
            var totalParticipants = await _bookingService.GetTotalParticipantsAsync(dto.TourID);
            var remaining = tour.Capacity - totalParticipants;

            if (dto.ParticipantCount > remaining)
            {
                TempData["Error"] = remaining <= 0
                    ? "Üzgünüz, bu tur için kontenjan dolmuştur."
                    : $"Yeterli kontenjan yok. Kalan kontenjan: {remaining} kişi.";
                return RedirectToAction("Create", new { id = dto.TourID });
            }

            dto.TotalPrice = dto.TourPrice * dto.ParticipantCount;
            var bookingNumber = await _bookingService.CreateBookingAsync(dto);

            TempData["Success"] = "Rezervasyonunuz başarıyla alındı!";
            return RedirectToAction("Confirmation", new { id = dto.TourID, bookingNumber = bookingNumber });
        }

        public IActionResult Confirmation(string id, string bookingNumber)
        {
            ViewBag.TourId = id;
            ViewBag.BookingNumber = bookingNumber;
            ViewBag.BreadcrumbTitle = "Rezervasyon Onayı";
            ViewBag.BreadcrumbParent = "Turlar";
            ViewBag.BreadcrumbParentUrl = "/Tour/TourList";
            return View();
        }
    }
}