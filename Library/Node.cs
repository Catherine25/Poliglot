namespace Library;

public class Node
{
    private readonly int _maxSentenceSize;

    public Node(string sentence, int maxSentenceSize)
    {
        Sentence = sentence;
        _maxSentenceSize = maxSentenceSize;
    }

    public string Sentence { get; set; }
    public Node First { get; set; }
    public Node Second { get; set; }

    public void SplitBy(char character)
    {
        // sentence is already smaller
        if (Sentence?.Length < _maxSentenceSize)
            return;

        int index = Sentence.IndexOf(character) + 1;

        // if character has not been found
        if (index == 0)
            return;

        string first = Sentence[..index];

        index++;

        if (index >= Sentence.Length)
            return;

        string second = Sentence[index..];

        First = new Node(first, _maxSentenceSize);
        Second = new Node(second, _maxSentenceSize);
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