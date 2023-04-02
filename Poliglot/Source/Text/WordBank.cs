namespace Poliglot.Source.Text;

public class WordBank
{
    public IEnumerable<Word> Words { get; set; } = new List<Word>();

    public void CompleteWord(Word word, bool correct)
    {
        Words.Single(w => w.Original == word.Original).Answered(correct);
    }

    public void RemoveByWord(string word)
    {
        Words = Words.Where(w => !string.Equals(w.Original, word, StringComparison.OrdinalIgnoreCase));
    }

    public void RemoveBySentence(string sentence)
    {
        Words = Words.Where(w => !string.Equals(w.Context, sentence, StringComparison.OrdinalIgnoreCase));
    }

    public void AddNote(Word word, string note)
    {
        Words.First(w => w == word).Note = note;
    }
}
