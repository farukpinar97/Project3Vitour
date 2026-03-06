using Microsoft.AspNetCore.Mvc;

namespace Project3Vitour.ViewComponents.HomeViewComponents
{
    public class _HomeFaqComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
