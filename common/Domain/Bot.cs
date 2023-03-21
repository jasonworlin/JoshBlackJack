namespace common.Domain;

public class Bot
{
    public int BotId { get; set; }
    public Hand Hand1 { get; set; }
    public Hand Hand2 { get; set; }
    public bool HasBusted { get; set; }
    public bool HasStuck { get; set; }
    public bool HasSplit { get; set; }
    public bool HasWon { get; set; }
    
    private Constants.ActiveHand _activeHand;
    private int _total;
    private readonly int StickMin;

    public Bot()
    {
        _total = 0;
        _activeHand = Constants.ActiveHand.Hand1;
        Hand1 = new Hand();
        Hand2 = new Hand();
        StickMin = 17;
    }
    
    public void PlayHand(Bot bot, Deck deck)
    {
        // Base case: bot has busted or stuck
        if (bot.HasBusted || bot.HasStuck)
        {
            return;
        }

        // Is the Bot doubling
        if (bot.IsDoubling())
        {
            // Bot doubles
            // Get one more card
            bot.ReceiveCard(deck.GetNextCard());
            return;
        }

        // Is the Bot hitting?
        if (bot.IsHitting())
        {
            // Get one more card
            bot.ReceiveCard(deck.GetNextCard());

            // Recursive call to continue the hand
            PlayHand(bot, deck);
        }
    }

    public void ReceiveCard(Card card)
    {
        // Get the current hand total
        var total = CalculateTotal();

        // Add the new card value to the total
        total += card.Value;

        // Check if the hand has busted
        if (total > Constants.BlackJackMax)
        {
            // The hand has busted
            HasBusted = true;

            // Add the card to the appropriate hand
            if (_activeHand == Constants.ActiveHand.Hand1)
                Hand1.Cards.Add(card);
            else
                Hand2.Cards.Add(card);

            // Exit the function, since the hand cannot be played anymore
            return;
        }

        // If the hand hasn't busted, update the total
        _total = total;

        // TODO: Make this more complicated to use the cards that have already gone to determine whether to stick or twist
        // Probability
        // Give players personalities, more risk, less risk

        // Check if the hand total is suffcient to stick
        if (_total >= StickMin)
        {
            HasStuck = true;
        }

        // Add the card to the appropriate hand
        if (_activeHand == Constants.ActiveHand.Hand1)
            Hand1.Cards.Add(card);
        else
            Hand2.Cards.Add(card);
    }

    public bool IsSplitting()
    {
        // Don't split twice
        if (HasSplit)
            return false;

        // TODO: Probability calculator for whether the user would split here or not
        if (Hand1.Cards[0].Value != Hand1.Cards[1].Value)
            return false;

        Split();
        return true;
    }

    private void Split()
    {
        // Set the bots split status
        HasSplit = true;

        // Create the 2nd hand
        Hand2 = new Hand();

        // Take the top card from hand 1 as the 1st card in hand 2
        Hand2.Cards.Add(Hand1.Cards[0]);

        // Remove the card from hand 1
        Hand1.Cards.RemoveAt(0);
    }

    public bool IsHitting()
    {
        return _total < StickMin;
    }

    public bool IsDoubling()
    {
        return _total is 10 or 11;
    }

    public void SetSecondHandActive()
    {
        // Reset the hand total for the 2nd hand
        _total = 0;

        // Set the second hand as active
        _activeHand = Constants.ActiveHand.Hand2;

        // Reset the status properties
        HasBusted = false;
        HasStuck = false;
    }

    public void CheckIfWon(Dealer dealer)
    {
        // If the dealer has busted or the bots total is greater than the dealer total then the bot has won
        if (dealer.HasBusted || CalculateTotal() > dealer.Total)
        {
            HasWon = true;
        }
    }

    private int CalculateTotal()
    {
        // In case the Bot has split, check which is the active hand and return the sum
        switch(_activeHand)
        {
            case Constants.ActiveHand.Hand2:
                return Hand2.Cards.Sum(card => card.Value);

            default:
                return Hand1.Cards.Sum(card => card.Value); 
        }
    }
}