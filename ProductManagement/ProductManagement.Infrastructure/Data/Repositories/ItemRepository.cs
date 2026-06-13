using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.Data;

namespace ProductManagement.Infrastructure.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Item>> GetAllByProductIdAsync(int productId)
        {
            // 1. Retrieves all items associated with the specified product ID without tracking for optimal read performance
            return await _context.Items
                .Where(x => x.ProductId == productId)
                .AsNoTracking()
                .ToListAsync();

        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            // 2. Fetches a specific item by its unique identifier without tracking, returning null if no match is found
            return await _context.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Item> CreateAsync(Item item)
        {
            // 3. Adds a new item entity to the database context for insertion
            await _context.Items.AddAsync(item);

            // 4. Persists the pending changes to the underlying database
            await _context.SaveChangesAsync();

            // 5. Returns the item entity containing any database-generated values like the new ID
            return item;

        }

        public async Task<Item> UpdateAsync(Item item)
        {
            // 6. Marks the provided item entity as modified within the database context
            _context.Items.Update(item);

            await _context.SaveChangesAsync();

            return item;

        }

        public async Task DeleteAsync(Item item)
        {
            // 7. Flags the specified item entity for deletion in the database context
            _context.Items.Remove(item);

            await _context.SaveChangesAsync();

        }

        public async Task<bool> ExistsAsync(int id)
        {
            // 8. Efficiently checks the database without tracking to see if any item matches the given ID
            return await _context.Items
                .AsNoTracking()
                .AnyAsync(x => x.Id == id);

        }
    }
}