using System.Text.Json.Serialization;

namespace common.Domain;

public class Player
{
    public int PlayerId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    //[JsonIgnore]
    public decimal Balance { get; set; }

    // TODO: Pull this out as Bot & Player use this
    private enum ActiveHand
    {
        Hand1,
        Hand2,
    }

    private ActiveHand _activeHand;
    private int _total;

    public Hand Hand1 { get; set; }
    public Hand Hand2 { get; set; }

    public Player()
    {
        _total = 0;
        _activeHand = ActiveHand.Hand1;
        Hand1 = new Hand();
        Hand2 = new Hand();
    }

    public Player(string? name, string? email, string? password) : base()
    {
        Name = name;
        Email = email;
        Password = password;
    }

    public bool HasBusted { get; set; }
    public bool HasStuck { get; set; }
    //public bool HasSplit { get; set; }
    public bool CanSplit { get; set; }
    public bool HasWon { get; set; }

    public void ReceiveCard(Card card)
    {
        //System.Console.WriteLine($"Player Card value {card.Value}");

        var (total1, total2) = CalculateTotal();

        //System.Console.WriteLine($"Player PRE TOTAL1 {total1}");
        //System.Console.WriteLine($"Player PRE TOTAL2 {total2}");

        total1 += card.Value;

        //System.Console.WriteLine($"Player POST TOTAL1 {total1}");
        //System.Console.WriteLine($"Player POST TOTAL2 {total2}");

        /*if (total2 > 0)
            total2 += card.Value;*/

        //if (total1 > 21 && total2 > 21)
        if (total1 > 21 /*&& total2 == 0*/)
        {
            //System.Console.WriteLine($"BUSTED");
            HasBusted = true;

            if (_activeHand == ActiveHand.Hand1)
                Hand1.Cards.Add(card);
            else
                Hand2.Cards.Add(card);

            return;
        }

        //_total = total2 > 21 ? total1 : total2;
        _total = total1;

        if (_activeHand == ActiveHand.Hand1)
        {
            Hand1.Cards.Add(card);

            if (Hand1.Cards.Count() == 2)
            {
                // Check whether the hand can be split
                if (Hand1.Cards[0].Value != Hand1.Cards[1].Value)
                    CanSplit = true;
            }
        }
        else
            Hand2.Cards.Add(card);
    }

    // public bool IsSplitting()
    // {
    //     // Don't split twice
    //     if (HasSplit)
    //         return false;

    //     // TODO: Probability calculator for whether the user would split here or not
    //     if (Hand1[0].Value != Hand1[1].Value)
    //         return false;

    //     Split();
    //     return true;
    // }

    public void Sticking()
    {
        HasStuck = true;
    }

    private void Split()
    {
        //HasSplit = true;
        Hand2 = new Hand();
        Hand2.Cards.Add(Hand1.Cards[0]);
        Hand1.Cards.RemoveAt(0);
    }

    private void SetSecondHandActive()
    {
        _total = 0;
        _activeHand = ActiveHand.Hand2;
        HasBusted = false;
        HasStuck = false;
    }

    private (int total1, int total2) CalculateTotal()
    {
        // TODO: Refactor 
        if (_activeHand == ActiveHand.Hand1)
        {
            System.Console.WriteLine("Hand 1");

            //if (Hand1.All(x => x.Value != 1))
            return (Hand1.Cards.Sum(card => card.Value), 0);

            // We have an Ace
            // This means there might be 2 totals!

            //System.Console.WriteLine("We have an Ace");

            var total1 = Hand1.Cards.Sum(card => card.Value);

            return (total1, total1 + 10);
        }
        else
        {
            if (Hand2.Cards.All(x => x.Value != 1))
                return (Hand2.Cards.Sum(card => card.Value), 0);

            // We have an Ace
            // This means there might be 2 totals!

            var total1 = Hand2.Cards.Sum(card => card.Value);

            return (total1, total1 + 10);
        }
    }

    public void CheckIfWon(Dealer dealer)
    {
        //System.Console.WriteLine($"Player dealer busted {dealer.HasBusted}");
        //System.Console.WriteLine($"{CalculateTotal().total1}, {dealer.Total}");
        if (dealer.HasBusted || CalculateTotal().total1 > dealer.Total)
        {
            System.Console.WriteLine("Player won");
            HasWon = true;
        }
    }    
}