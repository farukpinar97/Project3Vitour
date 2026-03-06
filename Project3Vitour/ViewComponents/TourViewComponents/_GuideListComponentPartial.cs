using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Services.GuideServices;

namespace Project3Vitour.ViewComponents.GuideViewComponents
{
    public class _GuideListComponentPartial : ViewComponent
    {
        private readonly IGuideService _guideService;

        public _GuideListComponentPartial(IGuideService guideService)
        {
            _guideService = guideService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _guideService.GetAllGuidesAsync();
            return View(values);
        }
    }
}