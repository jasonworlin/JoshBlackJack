namespace common.Domain;
public class Hand
{
    public int HandId { get; set; }
    public List<Card> Cards { get; set; }

    public Hand()
    {
        Cards = new List<Card>();
    }
}

