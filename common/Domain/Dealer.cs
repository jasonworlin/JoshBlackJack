namespace common.Domain;

public class Dealer
{
    public int DealerId { get; set; }
    public int Total { get; private set; }
    public Hand Hand { get; set; }
    public bool HasBusted { get; set; }
    public bool HasStuck { get; set; }
    public bool HasSplit { get; set; }

    private readonly int StickMin;
    
    public Dealer()
    {
        Total = 0;
        Hand = new Hand();
        StickMin = 16;
    }

    public void ReceiveCard(Card card)
    {
        // Get the current hand total
        Total = Hand.Cards.Sum(card => card.Value);

        // Add the new card value to the total
        Total += card.Value;

        // Has the dealer busted?
        if (Total > Constants.BlackJackMax)
        {
            // Busted!
            HasBusted = true;            
        }

        // Add the dealt card to the dealers hand
        Hand.Cards.Add(card);
    }

    public void PlayHand(Game game)
    {
        // Check if player has busted or stuck and exit function
        if (HasBusted || HasStuck)
            return;

        // If player wants to hit
        if (IsHitting())
        {
            // Draw card and add it to player's hand
            ReceiveCard(game.Deck.GetNextCard());

            // Recursively call PlayHand() to see if the player wants to hit again
            PlayHand(game);
        }
        else // Player wants to stick
        {
            // Set HasStuck to true and exit function
            HasStuck = true;
            return;
        }
    }

    private bool IsHitting()
    {
        return Total < StickMin;
    }
}