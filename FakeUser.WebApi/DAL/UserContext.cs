using FakeUser.Model;
using Microsoft.EntityFrameworkCore;

namespace FakeUser.WebApi.DAL;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}
