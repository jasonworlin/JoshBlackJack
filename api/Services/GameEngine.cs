using common.Domain;

namespace api.Services;

// TODO: 5 cards is a winning hand as well
// TODO: Need to be able to place bets and win money
// TODO: Do we want to save the game state in the database rather than POST back each time?

public class GameEngine
{
    public Game CreateGame(NewGame newGameConfig)
    {
        var game = new Game();

        CreatePlayers(game, newGameConfig);

        //AdjustPlayerBalance(newGameConfig);

        DealCards(game);

        return game;
    }

    // public void AdjustPlayerBalance(NewGame newGameConfig)
    // {

    // }

    public static void DealCards(Game game)
    {
        for (var i = 0; i < 2; i++)
        {
            foreach (var bot in game.Bots)
                bot.ReceiveCard(game.Deck.GetNextCard());

            game.Player.ReceiveCard(game.Deck.GetNextCard());
            game.Dealer.ReceiveCard(game.Deck.GetNextCard());
        }
    }

    internal static void PlayerTakeAHit(Game game)
    {
        if (game.Player == null)
        {
            // Somehow we don't have a player
            return;
        }

        game.Player.ReceiveCard(game.Deck.GetNextCard());
    }

    internal static void DealerPlay(Game game)
    {
        //System.Console.WriteLine("Dealer play hand");
        game.Dealer.PlayHand(game);

        // After the dealer has played we need to work out who has beat the dealer
        // Possible we have multiple winners (bots and player, just player, just bot or dealer)
        // Dealer might have bust!

        //if(game.Dealer.HasBusted)
        //    return;

        foreach (var bot in game.Bots)
        {
            if (bot.HasStuck)
                bot.CheckIfWon(game.Dealer);

            System.Console.WriteLine($"Dealer Play, has bot won {bot.HasWon}");
        }

        System.Console.WriteLine($"game player hand {game.Player.Hand1.Cards.Count()}");
        if (!game.Player.HasBusted)
        {
            game.Player.CheckIfWon(game.Dealer);
        }
    }

    public static int CalculateWinnings(Player player)
    {
        return ((player.BetPlaced / 2) * 3) + player.BetPlaced;
    }

    internal static void BotTakeATurn(Game game)
    {
        System.Console.WriteLine($"Game {game.GameId}");
        //System.Console.WriteLine($"CanSplit {game.Player.CanSplit}");
        System.Console.WriteLine($"No of bots {game.Bots.Count()}");
        var bot = game.Bots.First(p => !p.HasBusted && !p.HasStuck);

        if (bot == null)
        {
            System.Console.WriteLine($"All bots finished");
            // All players finished?
            return;
        }

        /* This is where we were
            The first bot was playing correctly but doesn't look like the second bot is taking a go
            so make sure the query above to get the bot is getting the correct one then see what is being
            returned when the hand is played for the second bot
        */

        System.Console.WriteLine($"Next plyr is {bot.Hand1.Cards.Count()}");

        bot.PlayHand(bot, game.Deck);

        if (!bot.HasSplit)
        {
            System.Console.WriteLine($"Has not split - returning {bot.Hand1.Cards.Count()}");
            return;
        }

        // Player has split so set the second hand as active
        bot.SetSecondHandActive();

        // Play the 2nd hand
        bot.PlayHand(bot, game.Deck);
    }



    private void CreateBots(Game game, int numberOfBots)
    {
        for (var i = 0; i < numberOfBots; i++)
            game.AddBot(new Bot());
    }

    private void CreatePlayers(Game game, NewGame newGameConfig)
    {
        CreateBots(game, newGameConfig.NumberOfBots);

        game.Player = new Player(newGameConfig.UserId);
        game.Dealer = new Dealer();
    }
}