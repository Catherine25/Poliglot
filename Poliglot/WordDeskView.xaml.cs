using System.Diagnostics;
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

        var sentenceParts = ProduceWords(word.Context, word.Original);

        var mappedWords = sentenceParts 
            .Select(w => (w, w.Contains(word.Original) ? word : null));

        Body.Clear();

        GenerateWordViewsForWord(mappedWords);
    }

    private IEnumerable<string> ProduceWords(string text, string studiedWord)
    {
        IEnumerable<string> wordsWithSeparators = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        wordsWithSeparators = wordsWithSeparators.SelectMany(word => EnsureNoSeparatorsInStudiedWord(word, studiedWord));

        return wordsWithSeparators;
    }

    private IEnumerable<string> EnsureNoSeparatorsInStudiedWord(string currentWord, string studiedWord)
    {
        // if we are not studying the word - we do not care about the separators from it
        if (currentWord != studiedWord)
            return new string[] { currentWord };

        // sentence separators collection
        char[] separators = new char[] { '.', ',', '\n' };

        // initialize result with the studied word
        List<string> strings = new()
        {
            studiedWord
        };

        // search separators and create buttons for them separately
        var index = studiedWord.IndexOfAny(separators);
        while (index != -1)
        {
            strings.Add(studiedWord.Substring(index, 1));
            index = studiedWord.IndexOfAny(separators, index + 1);
        }

        return strings;
    }

    public void GenerateWordViewsForWord(IEnumerable<(string contextWord, Word studiedWord)> mappedWords)
    {
        foreach (var item in mappedWords)
        {
            if (item.studiedWord == null)
            {
                Button label = new()
                {
                    Text = item.contextWord,
                    HeightRequest = 50,
                    VerticalOptions = LayoutOptions.Center,
                };

                Body.Add(label);
            }
            else
            {
                WordEntryView view = new(item.studiedWord);
                view.HeightRequest = 50;
                view.Completed += WordEntryView_Completed;
                Body.Add(view);
            }
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