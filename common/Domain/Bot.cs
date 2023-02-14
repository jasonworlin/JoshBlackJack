namespace common.Domain;

public enum PlayerType
{
    Bot,
    Player,
    Dealer
}

public class Bot
{
    public int BotId { get; set; }
    private enum ActiveHand
    {
        Hand1,
        Hand2,
    }

    private ActiveHand _activeHand;
    private int _total;

    public Hand Hand1 { get; set; }
    public Hand Hand2 { get; set; }

    public Bot()
    {
        _total = 0;
        _activeHand = ActiveHand.Hand1;
        Hand1 = new Hand();
        Hand2 = new Hand();
    }

    public bool HasBusted { get; set; }
    public bool HasStuck { get; set; }
    public bool HasSplit { get; set; }
    public bool HasWon { get; set; }

    public void PlayHand(Bot bot, Deck deck)
    {
        while (true)
        {
            if (bot.HasBusted || bot.HasStuck)
                break;

            //if (bot.PlayerType == PlayerType.Bot)
            {
                //player.IsSplitting();

                // Player double
                if (bot.IsDoubling())
                {
                    // Bet doubles
                    // Get one more card
                    bot.ReceiveCard(deck.GetNextCard());
                    break;
                }
            }

            if (bot.IsHitting())
                bot.ReceiveCard(deck.GetNextCard());
        }
    }

    public void ReceiveCard(Card card)
    {
        var (total1, total2) = CalculateTotal();
        total1 += card.Value;

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

        // TODO: Make this more complicated to use the cards that have already gone to determine whether to stick or twist
        // Probability
        // Give players personalities, more risk, less risk

        //System.Console.WriteLine($"TOTAL: {_total}");
        if (_total >= 17)
        {
            //System.Console.WriteLine("STICKIN");
            HasStuck = true;
        }


        if (_activeHand == ActiveHand.Hand1)
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
        HasSplit = true;
        Hand2 = new Hand();
        Hand2.Cards.Add(Hand1.Cards[0]);
        Hand1.Cards.RemoveAt(0);
    }

    public bool IsHitting()
    {
        return _total < 17;
    }

    public bool IsDoubling()
    {
        return _total is 10 or 11;
    }

    public void SetSecondHandActive()
    {
        _total = 0;
        _activeHand = ActiveHand.Hand2;
        HasBusted = false;
        HasStuck = false;
    }

    public void CheckIfWon(Dealer dealer)
    {
        if (dealer.HasBusted || CalculateTotal().total1 > dealer.Total)
            HasWon = true;
    }

    private (int total1, int total2) CalculateTotal()
    {
        // TODO: Refactor 
        if (_activeHand == ActiveHand.Hand1)
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
            if (Hand2.Cards.All(x => x.Value != 1))
                return (Hand2.Cards.Sum(card => card.Value), 0);

            // We have an Ace
            // This means there might be 2 totals!

            var total1 = Hand2.Cards.Sum(card => card.Value);

            return (total1, total1 + 10);
        }
    }
}