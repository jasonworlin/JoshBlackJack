using common.Domain;
using Microsoft.EntityFrameworkCore;

public partial class UserDb : DbContext
{
    public UserDb(DbContextOptions<UserDb> options)
            : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new User { UserId = 1, Email = "email@email.com", Name = "Jason", Password = "password", Balance = 100 });
    }
}