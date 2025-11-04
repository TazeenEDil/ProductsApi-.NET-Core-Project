using ProductsApi.Models;

namespace ProductsApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User> CreateAsync(User user);
        Task<User?> GetByIdAsync(int id);
    }
}
