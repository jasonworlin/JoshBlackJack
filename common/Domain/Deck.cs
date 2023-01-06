namespace common.Domain;
public class Deck
{
    public int DeckId { get; set; }
    private static Random rng = new Random();
    private readonly List<Card> _deck;

    public List<Card> Cards => _deck;

    public Deck()
    {        
        _deck = new List<Card>();
        var suits = new List<string> { "Hearts", "Clubs", "Diamonds", "Spades" };

        foreach (var suit in suits)
        {
            for (var i = 1; i <= 6; i++)
            {
                _deck.Add(new Card{Suit = suit, Value = i});

            }

            // Jack, Queen, King
            _deck.Add(new Card{Suit = suit, Value = 10});
            _deck.Add(new Card{Suit = suit, Value = 10});
            _deck.Add(new Card{Suit = suit, Value = 10});
        }

        Shuffle(_deck);        
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
        var card = _deck[_deck.Count - 1];
        _deck.RemoveAt(_deck.Count - 1);
        return card;
    }
}

