using Microsoft.AspNetCore.Mvc;
using MyStore.Business;
using MyStore.Services;  // Thêm để sử dụng ProductService
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; // Thêm dòng này để sử dụng DbUpdateConcurrencyException

namespace MyStore.WebMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService; // Inject ICategoryService
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);  // Trả về view Details.cshtml
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProductsAsync();
            return View(products);
        }

        // Hiển thị trang tạo sản phẩm
        public async Task<IActionResult> Create()
        {
            // Lấy danh sách các category từ ICategoryService
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllAsync(1, 100), "CategoryId", "CategoryName");
            return View();
        }

        // Tạo sản phẩm mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,CategoryId,UnitsInStock,UnitPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.SaveProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            // Lấy lại danh sách category khi gặp lỗi trong form
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllAsync(1, 100), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // Hiển thị trang chỉnh sửa sản phẩm
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync((int)id);
            if (product == null)
            {
                return NotFound();
            }

            // Lấy danh sách các category từ ICategoryService (dùng GetCategoriesAsync)
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllAsync(1, 100), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // Cập nhật sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,CategoryId,UnitsInStock,UnitPrice")] Product product)
        {
            if (id.ToString() != product.ProductId.ToString())
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.UpdateProductAsync(product); // Gọi ProductService để cập nhật sản phẩm
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productService.ProductExistsAsync(product.ProductId)) // Kiểm tra sự tồn tại của sản phẩm
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

            // Lấy lại danh sách category khi gặp lỗi trong form
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetAllAsync(1, 100), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // Xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product != null)
            {
                await _productService.DeleteProductAsync(product);
            }
            return RedirectToAction(nameof(Index)); // Điều hướng về trang Index sau khi xóa
        }

    }
}
