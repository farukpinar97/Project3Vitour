using Microsoft.AspNetCore.Mvc;

namespace Project3Vitour.ViewComponents.HomeViewComponents
{
    public class _HomeFeaturedTourComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
