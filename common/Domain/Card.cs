namespace common.Domain;

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

