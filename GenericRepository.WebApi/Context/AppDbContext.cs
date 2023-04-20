using GenericRepository.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GenericRepository.WebApi.Context;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) {}

    public DbSet<User> Users { get; set; }
}
