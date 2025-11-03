using ProductsApi.DTOs;
using Microsoft.AspNetCore.JsonPatch;

public interface IProductService
{
    Task<IEnumerable<ProductResponseDto>> GetAllProducts();
    Task<ProductResponseDto?> GetProductById(int id);
    Task<ProductResponseDto> CreateProduct(ProductDto dto);
    Task<bool> UpdateProduct(int id, ProductDto dto);
    Task<bool> PatchProduct(int id, JsonPatchDocument<ProductPatchDto> patchDoc);
    Task<bool> DeleteProduct(int id);
}
