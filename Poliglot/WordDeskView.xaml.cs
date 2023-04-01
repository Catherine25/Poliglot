using System.Diagnostics;
using System.Text.RegularExpressions;
using Poliglot.Source.Text;

namespace Poliglot;

public partial class WordDeskView : ContentView
{
    public Action<bool> WordCompleted;

    public Word Word
    {
        get => word;
        set
        {
            word = value;
            BuildFromWord();
        }
    }

    private Word word;

	public WordDeskView()
	{
		InitializeComponent();
	}

    private void BuildFromWord()
    {
        Debug.WriteLine($"Showing word '{word.Original}' in context '{word.Context}'");

        // for regex
        var context = word.Context + '\n';

        // for when word is at start
        string patternPartUpper = word.Original.StartWithUpperCase();

        string pattern = $"({patternPartUpper} | {word.Original} | {word.Original}[\\.,\\,\n])";

        IEnumerable<string> sentenceParts = Regex.Split(context, pattern);
        sentenceParts = sentenceParts.Where(p => p != string.Empty);

        var mappedWords = sentenceParts
            .Select(w => (w, w.Trim() == word.Original ? word : null));

        Body.Clear();

        GenerateWordViewsForWord(mappedWords);
    }

    public void GenerateWordViewsForWord(IEnumerable<(string contextWord, Word studiedWord)> mappedWords)
    {
        foreach (var item in mappedWords)
        {
            var view = item.studiedWord == null
                ? new WordEntryView(item.contextWord)
                : new WordEntryView(item.studiedWord);

            view.Completed += WordEntryView_Completed;

            Body.Add(view);
        }
    }

    private void WordEntryView_Completed(bool correct)
    {
        WordCompleted(correct);
    }

    public void ShowNoWords()
	{
        Body.Clear();
        Body.Add(new Label()
        {
            Text = "No words to repeat! 🙂",
            HeightRequest = 100
        });
    }

    public void Clear() => Body.Clear();
}