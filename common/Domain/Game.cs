namespace common.Domain;
public class Game
{
    public int GameId { get; set; }
    public Deck Deck { get; set; }
    public List<Bot> Bots { get; set; }
    public Player Player { get; set; }
    public Dealer Dealer { get; set; }

    public Game()
    {
        Deck = new Deck();
        Bots = new List<Bot>();
        //Player = new Player();
        Dealer = new Dealer();
    }

    public void AddBot(Bot bot)
    {
        Bots.Add(bot);
    }
}