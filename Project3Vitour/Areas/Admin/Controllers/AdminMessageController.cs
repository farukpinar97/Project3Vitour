using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Services.ContactServices;

namespace Project3Vitour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMessageController : Controller
    {
        private readonly IContactService _contactService;

        public AdminMessageController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<IActionResult> Index(int page = 1, string filter = "all")
        {
            const int pageSize = 10;

            ViewBag.Title = "Mesajlar";
            ViewBag.ActiveMenu = "messages";

            var allMessages = await _contactService.GetAllMessagesAsync();

            ViewBag.UnreadCount = allMessages.Count(x => !x.IsRead);
            ViewBag.TotalCount = allMessages.Count;

            // Filtre uygula
            var filtered = filter switch
            {
                "unread" => allMessages.Where(x => !x.IsRead).ToList(),
                "read" => allMessages.Where(x => x.IsRead).ToList(),
                _ => allMessages
            };

            int totalFiltered = filtered.Count;
            var paged = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalFiltered / (double)pageSize);
            ViewBag.CurrentFilter = filter;

            return View(paged);
        }

        public async Task<IActionResult> Detail(string id)
        {
            ViewBag.Title = "Mesaj Detayı";
            ViewBag.ActiveMenu = "messages";
            var message = await _contactService.GetMessageByIdAsync(id);
            if (!message.IsRead)
                await _contactService.MarkAsReadAsync(id);
            return View(message);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _contactService.DeleteMessageAsync(id);
            TempData["Success"] = "Mesaj başarıyla silindi!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MarkAsRead(string id)
        {
            await _contactService.MarkAsReadAsync(id);
            return RedirectToAction("Index");
        }
    }
}