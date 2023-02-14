using common.Domain;

namespace api.Services;

// TODO: 5 cards is a winning hand as well

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
        if (game.Player == null)
        {
            // Somehow we don't have a player
            return;
        }

        game.Player.ReceiveCard(game.Deck.GetNextCard());
    }

    internal static void DealerPlay(Game game)
    {
        game.Dealer.PlayHand(game);

        foreach (var bot in game.Bots)
        {
            if (bot.HasStuck)
                bot.CheckIfWon(game.Dealer);
        }

        if (!game.Player.HasBusted)
        {
            game.Player.CheckIfWon(game.Dealer);
        }
    }

    public static int CalculateWinnings(Player player)
    {
        return ((player.BetPlaced / 2) * 3) + player.BetPlaced;
    }

    internal static void BotPlayHand(Game game)
    {
        // Find the next bot which hasn't played a hand
        var bot = game.Bots.First(p => !p.HasBusted && !p.HasStuck);

        if (bot == null)
        {
            // All bots finished?
            return;
        }

        bot.PlayHand(bot, game.Deck);

        if (!bot.HasSplit)
        {
            return;
        }

        // Bot has split so set the second hand as active
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