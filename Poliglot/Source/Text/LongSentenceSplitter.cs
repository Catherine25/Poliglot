namespace Poliglot.Source.Text;

public class LongSentenceSplitter
{
    public const int MaxSize = 100;

    public IEnumerable<string> SplitBy(string sentence, char character)
    {
        if (sentence.Length > MaxSize)
        {
            int index = sentence.LastIndexOf(character) + 1;

            string first = sentence[..index];
            string second = sentence[index..];

            return new List<string> { first, second };
        }

        return new List<string> { sentence };
    }
}
