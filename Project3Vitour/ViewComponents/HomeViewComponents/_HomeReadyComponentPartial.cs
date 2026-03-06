using Microsoft.AspNetCore.Mvc;

namespace Project3Vitour.ViewComponents.HomeViewComponents
{
    public class _HomeReadyComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
