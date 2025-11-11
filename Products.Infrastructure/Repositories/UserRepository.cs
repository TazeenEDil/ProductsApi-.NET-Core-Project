using Microsoft.EntityFrameworkCore;
using Products.Application.Interfaces.Persistence;
using Products.Infrastructure.Data;
using Products.Domain.Entities;

namespace Products.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;

        public async Task<User?> GetByUsernameAsync(string username)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            }
            catch (Exception) { throw; }
        }

        public async Task<User> CreateAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception) { throw; }
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            try { return await _context.Users.FindAsync(id); }
            catch (Exception) { throw; }
        }
    }
}
