using System.Net.Security;

namespace api.Domain
{
    public class Deck
    {
        private static Random rng = new Random();
        private readonly Stack<Card> _deck;

        public Deck()
        {
            var deck = new List<Card>();
            var suits = new List<string> { "Hearts", "Clubs", "Diamonds", "Spades" };

            foreach (var suit in suits)
            {
                for (var i = 1; i <= 6; i++)
                {
                    deck.Add(new Card(suit, i));
                }

                // Jack, Queen, King
                deck.Add(new Card(suit, 10));
                deck.Add(new Card(suit, 10));
                deck.Add(new Card(suit, 10));
            }

            Shuffle(deck);
            _deck = new Stack<Card>(deck);
        }

        public void Shuffle(List<Card> deck)
        {
            var n = deck.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (deck[k], deck[n]) = (deck[n], deck[k]);
            }
        }

        public Card GetNextCard()
        {
            return _deck.Pop();
        }
    }

    public struct Card
    {
        private readonly string _suit;
        private readonly int _value;

        public Card(string suit, int value)
        {
            _suit = suit;
            _value = value;
        }

        public int Value
        {
            get { return _value; }
        }
    }
}
