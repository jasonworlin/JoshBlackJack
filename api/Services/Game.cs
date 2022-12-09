using api.Domain;

namespace api.Services;

public class Game
{
    private readonly Dealer _dealer;
    private readonly Deck _deck;
    private readonly List<Player> _players;

    public Game(int players)
    {
        _deck = new Deck();
        _dealer = new Dealer();
        _players = new List<Player>(players);
    }

    public (List<Player>, Dealer) NewGame()
    {
        CreatePlayers();

        Deal();

        foreach (var player in _players)
        {
            // TODO: Player places bet

            // Play the 1st hand
            PlayHand(player);

            if (!player.HasSplit)
                continue;

            // Player has split so set the second hand as active
            player.SetSecondHandActive();

            // Play the 2nd hand
            PlayHand(player);
        }

        // TODO: This is where the user would have their go!

        PlayHand(_dealer);

        return (_players, _dealer);
    }


    private void PlayHand(Player player)
    {
        while (true)
        {
            if (player.IsBusted || player.HasStuck)
                break;

            if (player.IsPlayer)
            {
                player.IsSplitting();

                // Player double
                if (player.IsDoubling())
                {
                    // Bet doubles
                    // Get one more card
                    player.ReceiveCard(_deck.GetNextCard());
                    break;
                }
            }

            if (player.IsHitting()) player.ReceiveCard(_deck.GetNextCard());
        }
    }

    private void CreatePlayers()
    {
        for (var i = 0; i < _players.Capacity; i++) _players.Add(new Player());
    }

    private void Deal()
    {
        for (var i = 0; i < 2; i++)
        {
            foreach (var player in _players) player.ReceiveCard(_deck.GetNextCard());

            _dealer.ReceiveCard(_deck.GetNextCard());
        }
    }
}