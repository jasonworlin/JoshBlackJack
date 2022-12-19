using common.Domain;

namespace api.Services;

public class GameEngine
{
    public Game CreateGame(int numberOfBots)
    {
        var game = new Game();

        CreatePlayers(game, numberOfBots);

        DealCards(game.Players, game.Deck);

        return game;
    }

    
    public static void DealCards(List<Player> players, Deck deck)
    {
        for (var i = 0; i < 2; i++)
        {
            foreach (var player in players)
                player.ReceiveCard(deck.GetNextCard());
        }
    }

    internal static void TakeATurn(Game game)
    {
        System.Console.WriteLine($"Game {game.Players.Count()}");
        // Get the next player whos turn it is
        var player = game.Players.First(p => !p.IsBusted && !p.HasStuck);

        if (player == null)
        {
            // All players finished?
            return;
        }

        System.Console.WriteLine($"Next plyr is {player.Hand1[0].Value} {player.Hand1[0].Suit}");

        PlayHand(player, game.Deck);

        if (!player.HasSplit)
            return;

        // Player has split so set the second hand as active
        player.SetSecondHandActive();

        // Play the 2nd hand
        PlayHand(player, game.Deck);
    }

    private static void PlayHand(Player player, Deck deck)
    {
        while (true)
        {
            if (player.IsBusted || player.HasStuck)
                break;

            if (player.PlayerType == PlayerType.Bot)
            {
                //player.IsSplitting();

                // Player double
                if (player.IsDoubling())
                {
                    // Bet doubles
                    // Get one more card
                    player.ReceiveCard(deck.GetNextCard());
                    break;
                }
            }

            if (player.IsHitting()) 
                player.ReceiveCard(deck.GetNextCard());
        }
    }

    

    private void CreateBots(Game game, int numberOfBots)
    {
        for (var i = 0; i < numberOfBots; i++)
            game.AddPlayer(new Player { PlayerType = PlayerType.Bot });
    }

    private void CreatePlayers(Game game, int numberOfBots)
    {
        CreateBots(game, numberOfBots);
        game.AddPlayer(new Player { PlayerType = PlayerType.Player });
        game.AddPlayer(new Player { PlayerType = PlayerType.Dealer });
    }
}