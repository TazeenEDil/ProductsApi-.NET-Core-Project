using ProductsApi.Models;

namespace ProductsApi.Repositories
{   //Product Repository interface inheriting from generic IRepository.
    //Contracts for data operations specific to Product entity are defined here.
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
