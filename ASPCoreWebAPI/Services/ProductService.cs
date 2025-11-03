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

        public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default)
        {
            return await _repository.GetAllAsync(cancellationToken);
        }

        public async Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _repository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _repository.AddAsync(product, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task<bool> UpdateProductAsync(int id, Product updatedProduct, CancellationToken cancellationToken = default)
        {
            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null) return false;

            existing.Name = updatedProduct.Name;
            existing.Description = updatedProduct.Description;
            existing.Price = updatedProduct.Price;

            _repository.Update(existing);
            await _repository.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> PatchProductAsync(int id, IDictionary<string, object> updates, CancellationToken cancellationToken = default)
        {
            var product = await _repository.GetByIdAsync(id, cancellationToken);
            if (product == null) return false;

            foreach (var item in updates)
            {
                var property = typeof(Product).GetProperty(item.Key);
                if (property != null)
                {
                    property.SetValue(product, Convert.ChangeType(item.Value, property.PropertyType));
                }
            }

            _repository.Update(product);
            await _repository.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await _repository.GetByIdAsync(id, cancellationToken);
            if (product == null) return false;

            _repository.Remove(product);
            await _repository.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
