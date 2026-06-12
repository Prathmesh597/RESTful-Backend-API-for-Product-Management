using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Interfaces
{

    public interface IItemRepository
    {

        // 1. Retrieves a list of all items that belong to a specific product ID
        Task<IEnumerable<Item>> GetAllByProductIdAsync(int productId);

        // 2. Fetches a single item by its unique identifier, returning null if it doesn't exist
        Task<Item?> GetByIdAsync(int id);

        // 3. Adds a new item record to the database
        Task<Item> CreateAsync(Item item);

        // 4. Modifies the details of an existing item in the database
        Task<Item> UpdateAsync(Item item);

        // 5. Deletes the specified item from the database
        Task DeleteAsync(Item item);

        // 6. Checks whether an item with the provided ID currently exists
        Task<bool> ExistsAsync(int id);

    }

}