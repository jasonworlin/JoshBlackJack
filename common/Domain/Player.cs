using System.Text.Json.Serialization;

namespace common.Domain;

public class Player : User
{
    public int PlayerId { get; set; }
    public Hand Hand1 { get; set; }
    public Hand Hand2 { get; set; }
    public int BetPlaced { get; set; }
    public bool HasBusted { get; set; }
    public bool HasStuck { get; set; }
    public bool CanSplit { get; set; }
    public bool HasWon { get; set; }

    private Constants.ActiveHand _activeHand;
    private int _total;

    public Player()
    {
        _total = 0;
        _activeHand = Constants.ActiveHand.Hand1;
        Hand1 = new Hand();
        Hand2 = new Hand();
    }

    public Player(int userId) : this()
    {
        UserId = userId;
    }

    public void ReceiveCard(Card card)
    {
        // Calculate the total value of the dealer's hand(s)
        var (total1, total2) = CalculateTotal();

        // Add the card value to the first hand total
        total1 += card.Value;

        // Check if the hand has busted
        if (total1 > Constants.BlackJackMax)
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
        _total = total1;

        // Add the card to the appropriate hand
        if (_activeHand == Constants.ActiveHand.Hand1)
        {
            Hand1.Cards.Add(card);

            // If this is the second card in the hand, check if it can be split
            if (Hand1.Cards.Count() == 2)
            {
                // Check whether the hand can be split (i.e., the two cards have the same value)
                if (Hand1.Cards[0].Value != Hand1.Cards[1].Value)
                    CanSplit = true;
            }
        }
        else
            Hand2.Cards.Add(card);
    }

    public void Split()
    {
        _activeHand = Constants.ActiveHand.Hand1;

        // Take the top card from hand 1 as the 1st card in hand 2        
        Hand2.Cards.Add(Hand1.Cards[0]);

        // Remove the card from hand 1
        Hand1.Cards.RemoveAt(0);
    }

    public void Sticking()
    {
        HasStuck = true;
    }

    private void SetSecondHandActive()
    {
        // Reset the hand total for the 2nd hand
        _total = 0;

        // Set the second hand as active
        _activeHand = Constants.ActiveHand.Hand2;

        // Reset the status properties
        HasBusted = false;
        HasStuck = false;
    }

    private (int total1, int total2) CalculateTotal()
    {
        // TODO: Refactor 
        if (_activeHand == Constants.ActiveHand.Hand1)
        {
            //if (Hand1.All(x => x.Value != 1))
            return (Hand1.Cards.Sum(card => card.Value), 0);

            // We have an Ace
            // This means there might be 2 totals!

            var total1 = Hand1.Cards.Sum(card => card.Value);

            return (total1, total1 + 10);
        }
        else
        {
            //if (Hand2.Cards.All(x => x.Value != 1))
            return (Hand2.Cards.Sum(card => card.Value), 0);

            // We have an Ace
            // This means there might be 2 totals!

            var total1 = Hand2.Cards.Sum(card => card.Value);

            return (total1, total1 + 10);
        }
    }

    public bool CheckIfWon(Dealer dealer)
    {
        // If the dealer has busted or the players total is greater than the dealer total then the player has won
        if (dealer.HasBusted || CalculateTotal().total1 > dealer.Total)
        {
            HasWon = true;
        }

        return HasWon;
    }
}