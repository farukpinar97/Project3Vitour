using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Project3Vitour.Controllers
{
    public class LanguageController : Controller
    {
        private static readonly HashSet<string> _supportedCultures = new() { "tr", "en", "de" };

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Set(string culture, string returnUrl = "/")
        {
            // Desteklenmeyen bir dil gelirse Türkçe'ye düşür
            if (string.IsNullOrWhiteSpace(culture) || !_supportedCultures.Contains(culture))
                culture = "tr";

            Response.Cookies.Append(
                ".Vitour.Culture", // Program.cs'deki CookieName ile aynı olmalı
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    HttpOnly = false,
                    SameSite = SameSiteMode.Lax
                }
            );

            // Güvenli redirect: sadece local URL'lere izin ver
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = "/";

            return LocalRedirect(returnUrl);
        }
    }
}