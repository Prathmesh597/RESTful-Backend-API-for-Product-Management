using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Interfaces
{
    public interface IProductService
    {
        // 1. Retrieves a complete list of all products formatted as response DTOs
        Task<IEnumerable<ProductResponseDto>> GetAllAsync();

        // 2. Fetches a single product by its unique identifier and returns it as a response DTO
        Task<ProductResponseDto> GetByIdAsync(int id);

        // 3. Processes the creation of a new product using the provided creation DTO
        Task<ProductResponseDto> CreateAsync(CreateProductDto dto);

        // 4. Applies updates to an existing product based on its ID and the provided update DTO
        Task<ProductResponseDto> UpdateAsync(int id, UpdateProductDto dto);

        // 5. Removes a product from the system using id
        Task DeleteAsync(int id);

    }
}