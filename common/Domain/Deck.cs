namespace common.Domain;
public class Deck
{
    public int DeckId { get; set; }
    private static Random rng = new Random();
    private readonly Stack<Card> _deck;

    public Stack<Card> Cards => _deck;

    public Deck()
    {
        var deck = new List<Card>();
        var suits = new List<string> { "Hearts", "Clubs", "Diamonds", "Spades" };

        foreach (var suit in suits)
        {
            for (var i = 1; i <= 6; i++)
            {
                deck.Add(new Card{Suit = suit, Value = i});

            }

            // Jack, Queen, King
            deck.Add(new Card{Suit = suit, Value = 10});
            deck.Add(new Card{Suit = suit, Value = 10});
            deck.Add(new Card{Suit = suit, Value = 10});
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

public class Card
{
    //private string _suit;
    //private int _value;

    public int CardId { get; set; }

    /*public Card(string suit, int value)
    {
        Suit = suit;
        Value = value;
    }*/

    public int Value
    {
        get; set;
        /*get { return _value; }
        set { _value = value; }*/
    }

    public string Suit
    {
        get; set;
        /*get { return _suit; }
        set { _suit = value; }*/
    }
}

