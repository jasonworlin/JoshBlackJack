using common.Domain;
using Microsoft.EntityFrameworkCore;

public partial class UserDb : DbContext
{
    public UserDb (DbContextOptions<UserDb> options)
            : base(options)
        {
        }    
    
    public DbSet<User> Users { get; set; } = null!;
}