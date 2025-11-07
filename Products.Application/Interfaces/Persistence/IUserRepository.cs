using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Products.Domain.Entities;

namespace Products.Application.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User> CreateAsync(User user);
        Task<User?> GetByIdAsync(int id);
    }
}
