using common.Domain;

namespace api.Services;

public interface IGameEngine
{
    Game CreateGame(NewGame newGameConfig);

    void DealCards(Game game);

    void PlayerTakeAHit(Game game);

    void DealerPlay(Game game);

    int CalculateWinnings(Player player);

    void BotPlayHand(Game game);
}

public class GameEngine : IGameEngine
{
    public Game CreateGame(NewGame newGameConfig)
    {
        // Create a new instance of the game
        var game = new Game();

        // Create the required game participants
        CreatePlayers(game, newGameConfig);

        // Deal the cards to the player
        DealCards(game);

        return game;
    }

    public void DealCards(Game game)
    {
        // The loop runs twice to deal two cards each
        for (var i = 0; i < 2; i++)
        {
            // Loop through each bot in the game and deal a card to them
            foreach (var bot in game.Bots)
            {
                bot.ReceiveCard(game.Deck.GetNextCard());
            }

            // Deal a card to the player
            game.Player.ReceiveCard(game.Deck.GetNextCard());

            // Deal a card to the dealer
            game.Dealer.ReceiveCard(game.Deck.GetNextCard());
        }
    }

    public void PlayerTakeAHit(Game game)
    {
        // Check the game has a valid player
        if (game.Player == null)
        {
            // Player not found so exit the function
            return;
        }

        // Give the player the top card from the deck 
        game.Player.ReceiveCard(game.Deck.GetNextCard());
    }

    public void DealerPlay(Game game)
    {
        // The dealer plays their hand 1st
        game.Dealer.PlayHand(game);

        // Loop through each bot in the game
        foreach (var bot in game.Bots)
        {
            // Check if the bot has stuck (i.e. decided to not receive any more cards)
            if (bot.HasStuck)
            {
                // If the bot has stuck, check if they have won against the dealer
                // Otherwise, the bot is still in the game and their turn will come after the dealer
                bot.CheckIfWon(game.Dealer);                
            }
        }

        // Check if the player has busted (i.e. exceeded a total of 21)
        if (!game.Player.HasBusted)
        {
            // If the player hasn't busted, check if they have won against the dealer
            game.Player.CheckIfWon(game.Dealer);
        }
        // If the player has busted, they have already lost and we don't need to check if they've won
    }

    public int CalculateWinnings(Player player)
    {
        return ((player.BetPlaced / 2) * 3) + player.BetPlaced;
    }

    public void BotPlayHand(Game game)
    {
        // Find the next bot which hasn't played a hand
        var bot = game.Bots.First(p => !p.HasBusted && !p.HasStuck);

        if (bot == null)
        {
            // All bots finished? Exist function
            return;
        }

        // Get the bot to play their hand
        bot.PlayHand(bot, game.Deck);

        // Check if the bot split while playing their hand
        if (!bot.HasSplit)
        {
            // They haven't split which means they will play eash hand separately so exit the function here
            return;
        }

        // Bot has split so set the second hand as active
        bot.SetSecondHandActive();

        // Play the 2nd hand
        bot.PlayHand(bot, game.Deck);
    }

    private void CreateBots(Game game, int numberOfBots)
    {
        // Create the requested number of bots 
        for (var i = 0; i < numberOfBots; i++)
            game.AddBot(new Bot());
    }

    private void CreatePlayers(Game game, NewGame newGameConfig)
    {
        // Create the game bots
        CreateBots(game, newGameConfig.NumberOfBots);

        // Create the player
        game.Player = new Player(newGameConfig.UserId);

        // Create the dealer
        game.Dealer = new Dealer();
    }
}