using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Project3Vitour.ViewComponents.TourViewComponents
{
    public class _TourHeaderComponentPartial : ViewComponent
    {
        private readonly IStringLocalizer<_TourHeaderComponentPartial> _localizer;

        public _TourHeaderComponentPartial(IStringLocalizer<_TourHeaderComponentPartial> localizer)
        {
            _localizer = localizer;
        }

        public IViewComponentResult Invoke()
        {
            ViewData["Localizer"] = _localizer;
            return View();
        }
    }
}