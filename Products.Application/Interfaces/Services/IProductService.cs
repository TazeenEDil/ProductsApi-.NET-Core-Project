using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Products.Application.DTOs.Product;

namespace Products.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
        Task<ProductResponseDto?> GetProductByIdAsync(int id);
        Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto);
        Task<bool> UpdateProductAsync(int id, ProductCreateDto dto);
        Task<bool> PatchProductAsync(int id, Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<ProductPatchDto> patchDoc);
        Task<bool> DeleteProductAsync(int id);
    }
}
