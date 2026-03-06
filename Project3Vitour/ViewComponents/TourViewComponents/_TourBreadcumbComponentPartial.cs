using Microsoft.AspNetCore.Mvc;

public class _TourBreadcumbComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}