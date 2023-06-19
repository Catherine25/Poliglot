namespace Poliglot.Source.Text;

public class LongSentenceSplitter
{
    public const int MaxSize = 100;
    private readonly string _initialSentence;
    private Node _node;

    public LongSentenceSplitter(string initialSentence)
    {
        _initialSentence = initialSentence;
        _node = new Node(_initialSentence);
    }

    public IEnumerable<string> SplitSentenceBy(char character)
    {
        _node.SplitBy(character);

        return _node.Construct();
    }
}

public class Node
{
    public const int MaxSize = 100;

    public Node(string sentence)
    {
        Sentence = sentence;
    }

    public string Sentence { get; set; }
    public Node First { get; set; }
    public Node Second { get; set; }

    public void SplitBy(char character)
    {
        if (Sentence?.Length < MaxSize)
            return;

        int index = Sentence.IndexOf(character) + 1;

        string first = Sentence[..index];

        index++;

        if (index >= Sentence.Length)
            return;

        string second = Sentence[index..];

        First = new Node(first);
        Second = new Node(second);
        Sentence = null;

        First.SplitBy(character);
        Second.SplitBy(character);
    }

    public IEnumerable<string> Construct()
    {
        if (Sentence != null)
            return new string[] { Sentence };

        var list = new List<string>();
        list.AddRange(First.Construct());
        list.AddRange(Second.Construct());

        return list;
    }
}