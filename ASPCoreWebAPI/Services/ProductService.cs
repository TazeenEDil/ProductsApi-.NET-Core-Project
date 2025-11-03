using ProductsApi.Models;
using ProductsApi.Repositories;

namespace ProductsApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository repo, ILogger<ProductService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name is required", nameof(product.Name));

            // Could include other business rules: price > 0, unique name, etc.
            await _repo.AddAsync(product, cancellationToken);
            await _repo.SaveChangesAsync(cancellationToken);

            return product;
        }

        public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default)
        {
            var existing = await _repo.GetByIdAsync(id, cancellationToken);
            if (existing == null) return false;

            _repo.Remove(existing);
            await _repo.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default)
        {
            return await _repo.GetAllAsync(cancellationToken);
        }

        public async Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _repo.GetByIdAsync(id, cancellationToken);
        }

        public async Task<bool> UpdateProductAsync(int id, Product updatedProduct, CancellationToken cancellationToken = default)
        {
            var existing = await _repo.GetByIdAsync(id, cancellationToken);
            if (existing == null) return false;

            // Business rules / validation example
            if (string.IsNullOrWhiteSpace(updatedProduct.Name))
                throw new ArgumentException("Product name cannot be empty");

            existing.Name = updatedProduct.Name;
            existing.Description = updatedProduct.Description;
            existing.Price = updatedProduct.Price;

            _repo.Update(existing);
            await _repo.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> PatchProductAsync(int id, IDictionary<string, object> updates, CancellationToken cancellationToken = default)
        {
            var existing = await _repo.GetByIdAsync(id, cancellationToken);
            if (existing == null) return false;

            foreach (var key in updates.Keys)
            {
                switch (key.ToLower())
                {
                    case "name":
                        existing.Name = updates[key]?.ToString() ?? existing.Name;
                        break;
                    case "description":
                        existing.Description = updates[key]?.ToString() ?? existing.Description;
                        break;
                    case "price":
                        if (double.TryParse(updates[key]?.ToString(), out var price))
                            existing.Price = price;
                        break;
                }
            }

            _repo.Update(existing);
            await _repo.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
