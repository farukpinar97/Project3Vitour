using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.ContactDtos;
using Project3Vitour.Services.ContactServices;

namespace Project3Vitour.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public IActionResult Index()
        {
            ViewBag.BreadcrumbTitle = "Bize Yazın";
            ViewBag.BreadcrumbParent = "Anasayfa";
            ViewBag.BreadcrumbParentUrl = "/";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateContactMessageDto dto)
        {
            await _contactService.CreateMessageAsync(dto);
            TempData["ContactResult"] = "success";
            return RedirectToAction("Index");
        }
    }
}