using AutoMapper;
using ProductManagement.Application.DTOs;
using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Exceptions;

namespace ProductManagement.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;

            _mapper = mapper;

        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
            // 1. Retrieves all existing products from the repository
            var products = await _productRepository.GetAllAsync();

            // 2. Maps the list of product entities to response DTOs and returns them
            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);

        }

        public async Task<ProductResponseDto> GetByIdAsync(int id)
        {
            // 3. Fetches the specific product by its ID to check for availability
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new NotFoundException("Product", id);

            // 4. Converts the found product entity into a response DTO
            return _mapper.Map<ProductResponseDto>(product);

        }

        public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
        {
            // 5. Maps the incoming creation DTO directly to a new product entity
            var product = _mapper.Map<Product>(dto);

            // 6. Sets the creation timestamp to the current UTC time
            product.CreatedOn = DateTime.UtcNow;

            // 7. Persists the newly created product entity to the database
            var created = await _productRepository.CreateAsync(product);

            // 8. Transforms the saved entity back into a response DTO for the client
            return _mapper.Map<ProductResponseDto>(created);

        }

        public async Task<ProductResponseDto> UpdateAsync(int id, UpdateProductDto dto)
        {
            // 9. Retrieves the existing product to ensure it is available for updates
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new NotFoundException("Product", id);

            // 10. Applies the updated values from the DTO onto the existing product entity
            _mapper.Map(dto, product);

            // 11. Updates the modification timestamp to the current UTC time
            product.ModifiedOn = DateTime.UtcNow;

            // 12. Commits the modified product entity to the database
            var updated = await _productRepository.UpdateAsync(product);

            // 13. Maps the successfully updated entity to a response DTO
            return _mapper.Map<ProductResponseDto>(updated);

        }

        public async Task DeleteAsync(int id)
        {
            // 14. Fetches the product intended for deletion to verify its existence
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new NotFoundException("Product", id);

            // 15. Removes the confirmed product from the database
            await _productRepository.DeleteAsync(product);

        }
    }
}