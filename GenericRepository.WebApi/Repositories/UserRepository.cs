using GenericRepository.WebApi.Context;
using GenericRepository.WebApi.Models;

namespace GenericRepository.WebApi.Repositories;

public sealed class UserRepository : Repository<User, AppDbContext>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) {}

    
}
