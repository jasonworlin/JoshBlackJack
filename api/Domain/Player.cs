namespace api.Domain
{
    public class Player
    {
        private enum ActiveHand
        {
            Hand1,
            Hand2,
        }

        private ActiveHand _activeHand;
        private int _total;
        
        public List<Card> Hand1;
        public List<Card> Hand2;

        public Player()
        {
            _total = 0;
            _activeHand = ActiveHand.Hand1;
            IsPlayer = true;
            IsBusted = false;
            HasStuck = false;
            HasSplit = false;
            Hand1 = new List<Card>();
            Hand2 = new List<Card>();
        }

        public bool IsPlayer { get; protected set; }
        public bool IsBusted { get; private set; }
        public bool HasStuck { get; private set; }
        public bool HasSplit { get; private set; }

        public void ReceiveCard(Card card)
        {
            var (total1, total2) = CalculateTotal();

            total1 += card.Value;

            if(total2 > 0)
                total2 += card.Value;

            if (total1 > 21 && total2 > 21)
            {
                IsBusted = true;
                return;
            }

            _total = total2 > 21 ? total1 : total2;

            // TODO: Make this more complicated to use the cards that have already gone to determine whether to stick or twist
            // Probability
            // Give players personalities, more risk, less risk
            if (_total >= 17)
                HasStuck = true;

            if(_activeHand == ActiveHand.Hand1)
                Hand1.Add(card);
            else
                Hand2.Add(card);
        }

        public bool IsSplitting()
        {
            // Don't split twice
            if (HasSplit) 
                return false;

            // TODO: Probability calculator for whether the user would split here or not
            if (Hand1[0].Value != Hand1[1].Value)
                return false;

            Split();
            return true;
        }

        private void Split()
        {
            HasSplit = true;
            Hand2 = new List<Card> { Hand1[0] };
            Hand1.RemoveAt(0);
        }

        public bool IsHitting()
        {
            return _total < 16;
        }

        public bool IsDoubling()
        {
            return _total is 10 or 11;
        }

        public void SetSecondHandActive()
        {
            _total = 0;
            _activeHand = ActiveHand.Hand2;
            IsBusted = false;
            HasStuck = false;
        }

        private (int total1, int total2) CalculateTotal()
        {
            // TODO: Refactor 
            if (_activeHand == ActiveHand.Hand1)
            {
                if (Hand1.All(x => x.Value != 1))
                    return (Hand1.Sum(card => card.Value), 0);

                // We have an Ace
                // This means there might be 2 totals!

                var total1 = Hand1.Sum(card => card.Value);

                return (total1, total1 + 10);
            }
            else
            {
                if (Hand2.All(x => x.Value != 1))
                    return (Hand2.Sum(card => card.Value), 0);

                // We have an Ace
                // This means there might be 2 totals!

                var total1 = Hand2.Sum(card => card.Value);

                return (total1, total1 + 10);
            }
        }
    }
}
