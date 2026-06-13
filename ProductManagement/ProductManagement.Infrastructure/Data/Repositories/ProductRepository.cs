using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.Data;

namespace ProductManagement.Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // 1. Retrieves all products along with their associated items, optimizing performance by disabling tracking
            return await _context.Products
                .Include(x => x.Items)
                .AsNoTracking()
                .ToListAsync();

        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            // 2. Fetches a specific product and its items by ID without tracking, returning null if no match is found
            return await _context.Products
                .Include(x => x.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Product> CreateAsync(Product product)
        {
            // 3. Adds a new product entity to the database context for insertion
            await _context.Products.AddAsync(product);

            // 4. Persists the pending changes to the underlying database
            await _context.SaveChangesAsync();

            // 5. Returns the product entity containing any database-generated values like the new ID
            return product;

        }

        public async Task<Product> UpdateAsync(Product product)
        {
            // 6. Marks the provided product entity as modified within the database context
            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return product;

        }

        public async Task DeleteAsync(Product product)
        {
            // 7. Flags the specified product entity for deletion in the database context
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

        }

        public async Task<bool> ExistsAsync(int id)
        {
            // 8. Efficiently checks the database without tracking to see if any product matches the given ID
            return await _context.Products
                .AsNoTracking()
                .AnyAsync(x => x.Id == id);

        }
    }
}