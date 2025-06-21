using Microsoft.AspNetCore.Mvc;
using MyStore.Business;  // Sử dụng các lớp trong MyStore.Business
using MyStore.Services;  // Sử dụng ICategoryService
using System.Threading.Tasks;

namespace MyStore.WebMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService; // Inject ICategoryService

        // Constructor để inject ICategoryService vào controller
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Hiển thị danh sách danh mục
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync(1, 100); // Lấy tất cả danh mục
            return View(categories);
        }

        // Thêm mới danh mục
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.AddAsync(category);  // Gọi ICategoryService để thêm mới danh mục
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // Chỉnh sửa danh mục
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetByIdAsync((int)id);  // Gọi ICategoryService để lấy danh mục theo ID
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.UpdateAsync(category);  // Gọi ICategoryService để cập nhật danh mục
                }
                catch (Exception)
                {
                    if (await _categoryService.GetByIdAsync(category.CategoryId) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // Xóa danh mục
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category != null)   
            {
                await _categoryService.DeleteAsync(id);  // Gọi ICategoryService để xóa danh mục
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
