namespace Poliglot.Source.Text;

public class LongSentenceSplitter
{
    public const int MaxSize = 100;

    public IEnumerable<string> SplitSentence(string sentence, char character)
    {
        if (CanContinue(sentence, character))
            return new List<string> { sentence };

        var parts = GetParts(sentence, character);

        var result = new List<string>();

        if (!string.IsNullOrEmpty(parts.first))
            result.AddRange(SplitSentence(parts.first, character));

        if (!string.IsNullOrEmpty(parts.second))
            result.AddRange(SplitSentence(parts.second, character));

        return result;
    }

    private bool CanContinue(string sentence, char character)
    {
        return sentence.Length < MaxSize || !sentence.Contains(character);
    }

    private (string first, string second) GetParts(string sentence, char character)
    {
        int index = sentence.IndexOf(character) + 1;

        string first = sentence[..index];

        index++;

        string second = sentence[index..];

        return (first, second);
    }
}
