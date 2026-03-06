using Microsoft.AspNetCore.Mvc;

namespace Project3Vitour.ViewComponents.HomeViewComponents
{
    public class _HomeWhyChoiseUsComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
