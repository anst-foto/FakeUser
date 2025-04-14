using FakeUser.Model;
using Microsoft.EntityFrameworkCore;

namespace FakeUser.WebApi.DAL;

public class UserContext : DbContext
{
    private readonly string _connectionString;
    
    public DbSet<User> Users { get; set; }

    public UserContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}