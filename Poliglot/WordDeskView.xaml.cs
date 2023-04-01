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

        var context = word.Context;
        var mappedWords = context
            .Split(' ') // todo use text processor
            .Select(w => (w, w == word.Original ? word : null));

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

    public void AddBlockButton()
    {
        Button blockButton = new()
        {
            Text = "❌",
            HeightRequest = 100
        };

        Body.Add(blockButton);
    }

    public void Clear() => Body.Clear();
}