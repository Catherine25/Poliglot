namespace Library;

public class LongSentenceSplitter
{
    private readonly string _initialSentence;
    private readonly int _maxSentenceSize;
    private Node _node;

    public LongSentenceSplitter(string initialSentence, int maxSentenceSize)
    {
        _initialSentence = initialSentence;
        _maxSentenceSize = maxSentenceSize;

        _node = new Node(_initialSentence, _maxSentenceSize);
    }

    public IEnumerable<string> SplitSentenceBy(char character)
    {
        _node.SplitBy(character);

        return _node.Construct();
    }
}
