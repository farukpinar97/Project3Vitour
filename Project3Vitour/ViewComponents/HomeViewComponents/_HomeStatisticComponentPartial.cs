using Microsoft.AspNetCore.Mvc;

namespace Project3Vitour.ViewComponents.HomeViewComponents
{
    public class _HomeStatisticComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
