using Microsoft.EntityFrameworkCore;
using MyStore.BusinessObjects.Models;

namespace MyStore.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsWithCategoryAsync();
    }

    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly MyStoreDbContext _context;

        public ProductRepository(MyStoreDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsWithCategoryAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }
    }
}