namespace common.Domain;
public class Deck
{
    public int DeckId { get; set; }
    public List<Card> Cards => _deck;

    private static Random randomiser = new Random();
    private readonly List<Card> _deck;    

    public Deck()
    {
        // Create a new deck
        _deck = new List<Card>();

        // Initialise the suits
        var suits = new List<string> { "Hearts", "Clubs", "Diamonds", "Spades" };

        // Initialise a full deck
        InitialiseDeck(suits);

        // Shuffle the deck
        Shuffle(_deck);
    }

    private void InitialiseDeck(List<string> suits)
    {
        // For each suit        
        foreach (var suit in suits)
        {
            // Add the cards from 1 to 10
            for (var i = 1; i <= 10; i++)
            {
                _deck.Add(new Card { Suit = suit, Value = i });
            }

            // Add the Jack, Queen and King
            for (var i = 0; i < 3; i++)
            {
                _deck.Add(new Card { Suit = suit, Value = 10 });
            }
        }
    }

    public void Shuffle(List<Card> deck)
    {
        // Get the number of cards in the deck
        var noOfCards = deck.Count;

        // While there are still cards to shuffle
        while (noOfCards > 1)
        {
            // Decrement the number of cards left to shuffle
            noOfCards--;

            // Generate a random number between 0 and noOfCards, inclusive
            var randomPosition = randomiser.Next(noOfCards + 1);

            // Swap the card at index randomPosition with the card at index noOfCards
            // This shuffles the deck by swapping cards at random positions
            (deck[randomPosition], deck[noOfCards]) = (deck[noOfCards], deck[randomPosition]);
        }
    }


    public Card GetNextCard()
    {
        // Get the card at the top of the deck
        var card = _deck[_deck.Count - 1];

        // Remove the card at the top of the deck
        _deck.RemoveAt(_deck.Count - 1);

        return card;
    }
}

