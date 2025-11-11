using Microsoft.AspNetCore.JsonPatch;
using ProductsApi.DTOs;
using ProductsApi.Models;
using ProductsApi.Repositories;

namespace ProductsApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProducts()
        {
            try
            {
                var products = await _repository.GetAllAsync();
                return products.Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductResponseDto?> GetProductById(int id)
        {
            try
            {
                var product = await _repository.GetByIdAsync(id);
                if (product == null) return null;

                return new ProductResponseDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductResponseDto> CreateProduct(ProductDto dto)
        {
            try
            {
                var newProduct = new Product
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price
                };

                var created = await _repository.AddAsync(newProduct);

                return new ProductResponseDto
                {
                    Id = created.Id,
                    Name = created.Name,
                    Description = created.Description,
                    Price = created.Price
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateProduct(int id, ProductDto dto)
        {
            try
            {
                var product = await _repository.GetByIdAsync(id);
                if (product == null) return false;

                product.Name = dto.Name;
                product.Description = dto.Description;
                product.Price = dto.Price;

                return await _repository.UpdateAsync(product);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> PatchProduct(int id, JsonPatchDocument<ProductPatchDto> patchDoc)
        {
            try
            {
                var product = await _repository.GetByIdAsync(id);
                if (product == null) return false;

                var patchDto = new ProductPatchDto
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price
                };

                patchDoc.ApplyTo(patchDto);

                product.Name = patchDto.Name ?? product.Name;
                product.Description = patchDto.Description ?? product.Description;
                product.Price = patchDto.Price ?? product.Price;

                return await _repository.UpdateAsync(product);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
