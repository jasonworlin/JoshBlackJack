using common.Domain;
using Microsoft.EntityFrameworkCore;

public partial class GameDb : DbContext
{
    public GameDb(DbContextOptions<GameDb> options)
            : base(options)
    {
    }

    public DbSet<Game> Games { get; set; } = null!;
    //public DbSet<Deck> Decks { get; set; }
    //public DbSet<Bot>? Bots { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Hand> Hands { get; set; }
    //public DbSet<Dealer>? Dealers { get; set; }
    //public DbSet<Card>? Cards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Game>().HasMany(x => x.Bots);
        //modelBuilder.Entity<Game>().HasOne(p => p.Player);
        //modelBuilder.Entity<Player>().HasOne(p => p.Hand1);
        //modelBuilder.Entity<Player>().HasOne(p => p.Hand2);
        //modelBuilder.Entity<Game>().ToTable("Games");
        modelBuilder.Entity<Deck>().ToTable("Decks");
        modelBuilder.Entity<Bot>().ToTable("Bots");
        //modelBuilder.Entity<Player>().ToTable("Players");
        //modelBuilder.Entity<Hand>().ToTable("Hand");
        modelBuilder.Entity<Dealer>().ToTable("Dealers");
        modelBuilder.Entity<Card>().ToTable("Cards");
    }
}