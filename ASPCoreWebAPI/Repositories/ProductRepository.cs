using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.Models;
using System.Linq.Expressions;

namespace ProductsApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Product> _db;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
            _db = _context.Set<Product>();
        }

        public async Task AddAsync(Product entity, CancellationToken cancellationToken = default)
        {
            await _db.AddAsync(entity, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await _db.FindAsync(new[] { id }, cancellationToken);
        }

        // strongly typed override
        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _db.FindAsync(new object[] { id }, cancellationToken);
        }

        public void Remove(Product entity)
        {
            _db.Remove(entity);
        }

        public void Update(Product entity)
        {
            _db.Update(entity);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _db.Where(predicate).AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
