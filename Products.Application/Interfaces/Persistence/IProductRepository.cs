using Products.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products.Application.Interfaces.Persistence
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }
}
