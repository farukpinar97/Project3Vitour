using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.ReviewDtos;
using Project3Vitour.Services.TourServices;
using Project3Vitour.Services.ModerationServices;

public class TourController : Controller
{
    private readonly ITourService _tourService;
    private readonly ITourReviewService _reviewService;
    private readonly IModerationService _moderationService;

    public TourController(
        ITourService tourService,
        ITourReviewService reviewService,
        IModerationService moderationService)
    {
        _tourService = tourService;
        _reviewService = reviewService;
        _moderationService = moderationService;
    }

    public async Task<IActionResult> TourList(int page = 1, string sort = "")
    {
        var allTours = await _tourService.GetAllTourAsync();

        // Sıralama
        allTours = sort switch
        {
            "price_asc" => allTours.OrderBy(x => x.Price).ToList(),
            "price_desc" => allTours.OrderByDescending(x => x.Price).ToList(),
            "new" => allTours.OrderByDescending(x => x.TourID).ToList(),
            _ => allTours
        };

        int pageSize = 6;
        int totalCount = allTours.Count;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var pagedTours = allTours.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.TotalCount = totalCount;
        ViewBag.PageSize = pageSize;
        ViewBag.CurrentSort = sort;
        ViewBag.BreadcrumbTitle = "Güncel Turlar";
        ViewBag.BreadcrumbParent = "Anasayfa";
        ViewBag.BreadcrumbParentUrl = "/";

        return View(pagedTours);
    }

    public IActionResult TourDetail(string id)
    {
        ViewBag.TourId = id;
        ViewBag.BreadcrumbTitle = "Tur Detayı";
        ViewBag.BreadcrumbParent = "Turlar";
        ViewBag.BreadcrumbParentUrl = "/Tour/TourList";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddReview(CreateReviewDto dto)
    {
        var (isClean, reason) = await _moderationService.AnalyzeAsync(dto.Comment);
        dto.IsApproved = isClean;
        dto.ModerationNote = reason;
        await _reviewService.CreateReviewAsync(dto);

        if (isClean)
            TempData["ReviewResult"] = "success";
        else
            TempData["ReviewResult"] = "rejected";

        return Redirect($"/Tour/TourDetail/{dto.TourID}?tab=reviews");
    }

    [HttpGet]
    public async Task<IActionResult> LoadMoreReviews(string tourId, int skip = 0, int take = 3)
    {
        var reviews = await _reviewService.GetReviewsByTourIdPagedAsync(tourId, skip, take);
        return Json(reviews);
    }
}