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

        // for regex
        var context = word.Context;

        IEnumerable<string> sentenceParts = context.Split(' ', '.', ',', '\n');
        sentenceParts = sentenceParts.Where(p => p != string.Empty);

        var mappedWords = sentenceParts
            .Select(w => (w, w == word.Original ? word : null));

        Body.Clear();

        GenerateWordViewsForWord(mappedWords);
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