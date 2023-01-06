using common.Domain;
using Microsoft.EntityFrameworkCore;

public partial class GameDb : DbContext
{
    public GameDb(DbContextOptions<GameDb> options)
            : base(options)
    {
    }

    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<Deck> Decks { get; set; }
    //public DbSet<Bot>? Bots { get; set; }
    //public DbSet<Player>? Players { get; set; }
    //public DbSet<Dealer>? Dealers { get; set; }
    //public DbSet<Card>? Cards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>().ToTable("Games");
        modelBuilder.Entity<Deck>().ToTable("Decks");
        //modelBuilder.Entity<Bot>().ToTable("Bots");
        //modelBuilder.Entity<Player>().ToTable("Players");
        //modelBuilder.Entity<Dealer>().ToTable("Dealers");
        //modelBuilder.Entity<Card>().ToTable("Card");
    }
}