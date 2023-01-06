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

        DealCards(game);

        return game;
    }

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
        /*if (game.Player == null)
        {
            // Somehow we don't have a player
            return;
        }*/

        //game.Player.ReceiveCard(game.Deck.GetNextCard());
    }

    internal static void DealerPlay(Game game)
    {
        //System.Console.WriteLine("Dealer play hand");
        //game.Dealer.PlayHand(game);

        // After the dealer has played we need to work out who has beat the dealer
        // Possible we have multiple winners (bots and player, just player, just bot or dealer)
        // Dealer might have bust!

        //if(game.Dealer.HasBusted)
        //    return;

        /*foreach (var bot in game.Bots)
        {            
            if(bot.HasStuck)
                bot.CheckIfWon(game.Dealer);

            //System.Console.WriteLine($"Dealer Play, has bot won {bot.HasWon}");
        }*/
        
            
        //System.Console.WriteLine($"game player hand {game.Player.Hand1.Count()}");
        //if(!game.Player.HasBusted)
        //    game.Player.CheckIfWon(game.Dealer);        
    }

    internal static void BotTakeATurn(Game game)
    {
        Bot bot = null;//game.Bots.First(p => !p.HasBusted && !p.HasStuck);

        if (bot == null)
        {
            // All players finished?
            return;
        }

        //System.Console.WriteLine($"Next plyr is {bot.Hand1[0].Value} {bot.Hand1[0].Suit}");

        //bot.PlayHand(bot, game.Deck);

        if (!bot.HasSplit)
            return;

        // Player has split so set the second hand as active
        bot.SetSecondHandActive();

        // Play the 2nd hand
        //bot.PlayHand(bot, game.Deck);
    }



    private void CreateBots(Game game, int numberOfBots)
    {
        for (var i = 0; i < numberOfBots; i++)
            game.AddBot(new Bot() /*{ PlayerType = PlayerType.Bot }*/);
    }

    private void CreatePlayers(Game game, NewGame newGameConfig)
    {
        CreateBots(game, newGameConfig.NumberOfBots);

        game.Player = new Player(newGameConfig.UserId);
        //game.Dealer = new Dealer();
    }
}