using AutoMapper;
using ProductManagement.Application.DTOs;
using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Exceptions;

namespace ProductManagement.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        private readonly IProductRepository _productRepository;

        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepository, IProductRepository productRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;

            _productRepository = productRepository;

            _mapper = mapper;

        }

        public async Task<IEnumerable<ItemResponseDto>> GetAllByProductIdAsync(int productId)
        {
            // 1. Validates that the requested product exists before fetching its items
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                throw new NotFoundException("Product", productId);

            // 2. Retrieves all items associated with the validated product ID
            var items = await _itemRepository.GetAllByProductIdAsync(productId);

            // 3. Maps the resulting item entities to response DTOs and returns them
            return _mapper.Map<IEnumerable<ItemResponseDto>>(items);

        }

        public async Task<ItemResponseDto> GetByIdAsync(int id)
        {
            // 4. Fetches the requested item by its ID to ensure it exists
            var item = await _itemRepository.GetByIdAsync(id);

            if (item == null)
                throw new NotFoundException("Item", id);

            // 5. Converts the found item entity into a response DTO
            return _mapper.Map<ItemResponseDto>(item);

        }

        public async Task<ItemResponseDto> CreateAsync(CreateItemDto dto)
        {
            // 6. Verifies that the product specified in the creation DTO actually exists
            var product = await _productRepository.GetByIdAsync(dto.ProductId);

            if (product == null)
                throw new NotFoundException("Product", dto.ProductId);

            // 7. Maps the incoming creation DTO to a domain entity
            var item = _mapper.Map<Item>(dto);

            // 8. Persists the new item entity to the database
            var created = await _itemRepository.CreateAsync(item);

            // 9. Maps the successfully saved item back to a response DTO
            return _mapper.Map<ItemResponseDto>(created);

        }

        public async Task<ItemResponseDto> UpdateAsync(int id, UpdateItemDto dto)
        {
            // 10. Retrieves the existing item to confirm it is available for updating
            var item = await _itemRepository.GetByIdAsync(id);

            if (item == null)
                throw new NotFoundException("Item", id);

            // 11. Applies the updated values from the DTO onto the existing item entity
            _mapper.Map(dto, item);

            // 12. Commits the updated item entity to the database
            var updated = await _itemRepository.UpdateAsync(item);

            // 13. Transforms the updated entity into a response DTO for the caller
            return _mapper.Map<ItemResponseDto>(updated);

        }

        public async Task DeleteAsync(int id)
        {
            // 14. Fetches the item intended for deletion to verify its existence
            var item = await _itemRepository.GetByIdAsync(id);

            if (item == null)
                throw new NotFoundException("Item", id);

            // 15. Removes the confirmed item from the database
            await _itemRepository.DeleteAsync(item);

        }
    }
}