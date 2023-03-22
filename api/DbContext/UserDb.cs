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
        // TESTING: Added default user
        modelBuilder.Entity<User>().HasData(new User { UserId = 1, Email = "email@email.com", Name = "Jason", HashedPassword = "password", Balance = 100 });
    }
}