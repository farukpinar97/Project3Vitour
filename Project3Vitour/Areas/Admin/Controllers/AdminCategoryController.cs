using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.CategoryDtos;
using Project3Vitour.Services.CategoryServices;

namespace Project3Vitour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public AdminCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page = 1, string filter = "all")
        {
            const int pageSize = 10;
            ViewBag.Title = "Kategoriler";
            ViewBag.ActiveMenu = "categories";

            var allCategories = await _categoryService.GetAllCategoryAsync();

            ViewBag.TotalCount = allCategories.Count;
            ViewBag.ActiveCount = allCategories.Count(x => x.CategoryStatus);
            ViewBag.PassiveCount = allCategories.Count(x => !x.CategoryStatus);

            var filtered = filter switch
            {
                "active" => allCategories.Where(x => x.CategoryStatus).ToList(),
                "passive" => allCategories.Where(x => !x.CategoryStatus).ToList(),
                _ => allCategories
            };

            var paged = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(filtered.Count / (double)pageSize);
            ViewBag.CurrentFilter = filter;

            return View(paged);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Yeni Kategori";
            ViewBag.ActiveMenu = "categories";
            return View(new CreateCategoryDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            await _categoryService.CreateCategoryAsync(dto);
            TempData["Success"] = "Kategori başarıyla eklendi!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            ViewBag.Title = "Kategori Düzenle";
            ViewBag.ActiveMenu = "categories";
            var category = await _categoryService.GetCategoryByIdAsync(id);
            var dto = new UpdateCategoryDto
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName,
                CategoryStatus = category.CategoryStatus
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryDto dto)
        {
            await _categoryService.UpdateCategoryAsync(dto);
            TempData["Success"] = "Kategori başarıyla güncellendi!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            TempData["Success"] = "Kategori başarıyla silindi!";
            return RedirectToAction("Index");
        }
    }
}