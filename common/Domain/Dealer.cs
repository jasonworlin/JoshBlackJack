namespace common.Domain
{
    public class Dealer
    {
        public int DealerId { get; set; }
        public int Total{ get; private set;}

        public List<Card> Hand { get; set; }

        public Dealer()
        {
            Total = 0;
            Hand = new List<Card>();
        }

        public bool HasBusted { get; set; }
        public bool HasStuck { get; set; }
        public bool HasSplit { get; set; }
        
        public void ReceiveCard(Card card)
        {
            //System.Console.WriteLine($"Dealer PRE TOTAL {Total}");

            Total = Hand.Sum(card => card.Value);
            Total += card.Value;

            //System.Console.WriteLine($"Dealer POST TOTAL {Total}");

            if (Total > 21)
            {
                System.Console.WriteLine($"BUSTED");
                HasBusted = true;

                Hand.Add(card);

                return;
            }

            Hand.Add(card);
        }

        public void PlayHand(Game game)
        {            
            while (true)
            {
                if (HasBusted || HasStuck)
                    break;

                if (IsHitting())
                    ReceiveCard(game.Deck.GetNextCard());
                else
                    HasStuck = true;
            }
        }

        private bool IsHitting()
        {
            return Total < 16;
        }
    }
}
