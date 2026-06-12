using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Interfaces
{
    public interface IProductRepository
    {
        // 1. Retrieves a complete list of all products currently in the database
        Task<IEnumerable<Product>> GetAllAsync();

        // 2. Fetches a single product by its unique identifier, returning null if not found
        Task<Product?> GetByIdAsync(int id);

        // 3. Adds a new product record to the database
        Task<Product> CreateAsync(Product product);

        // 4. Modifies the details of an existing product in the database
        Task<Product> UpdateAsync(Product product);

        // 5. Deletes the specified product from the database
        Task DeleteAsync(Product product);

        // 6. Checks whether a product with the provided ID currently exists
        Task<bool> ExistsAsync(int id);

    }
}