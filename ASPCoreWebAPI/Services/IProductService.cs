using ProductsApi.Models;

namespace ProductsApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default);
        Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken = default);
        Task<bool> UpdateProductAsync(int id, Product updatedProduct, CancellationToken cancellationToken = default);
        Task<bool> PatchProductAsync(int id, IDictionary<string, object> updates, CancellationToken cancellationToken = default);
        Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default);
    }
}
