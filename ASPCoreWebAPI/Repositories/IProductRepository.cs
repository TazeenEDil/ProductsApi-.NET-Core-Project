using ProductsApi.Models;
using System.Linq.Expressions;

namespace ProductsApi.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Product product, CancellationToken cancellationToken = default);
        void Update(Product product);
        void Remove(Product product);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        // Optional: To filter/search products
        Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
