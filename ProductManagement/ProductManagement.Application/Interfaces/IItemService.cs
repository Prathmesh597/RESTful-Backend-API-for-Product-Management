using ProductManagement.Application.Interfaces;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Interfaces
{
    public interface IItemService
    {
        // 1. Retrieves a collection of items formatted as response DTOs for a given product ID
        Task<IEnumerable<ItemResponseDto>> GetAllByProductIdAsync(int productId);

        // 2. Fetches a single item by its unique identifier and returns it as a response DTO
        Task<ItemResponseDto> GetByIdAsync(int id);

        // 3. Processes the creation of a new item using the provided creation DTO
        Task<ItemResponseDto> CreateAsync(CreateItemDto dto);

        // 4. Applies updates to an existing item based on its ID and the provided update DTO
        Task<ItemResponseDto> UpdateAsync(int id, UpdateItemDto dto);

        // 5. Removes an item from the system using its unique identifier
        Task DeleteAsync(int id);

    }
}