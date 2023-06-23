using System.Diagnostics;
using Library;
using Poliglot.Source.Text;

namespace Poliglot;

public partial class WordDeskView : ContentView
{
    public Action<bool> WordCompleted;

    public WordInContext Word
    {
        get => word;
        set
        {
            word = value;
            BuildFromWord();
        }
    }

    private WordInContext word;

	public WordDeskView()
	{
		InitializeComponent();
	}

    private void BuildFromWord()
    {
        Debug.WriteLine($"Showing word '{word.Original}' in context '{word.Context}'");
        Debug.WriteLine($"Repeat time: '{word.RepeatTime}'");

        var targetWordExtractor = new TargetWordExtractor();
        var sentenceParts = targetWordExtractor.ProduceWords(word.Context, word.Original);

        var shortened = SplitLongSentencesBy(sentenceParts, Delimeters.Comma);

        var mappedWords = shortened
            .Select(w => (w, w == word.Original ? word : null));

        Body.Clear();

        GenerateWordViewsForWord(mappedWords);
    }

    private IEnumerable<string> SplitLongSentencesBy(IEnumerable<string> sentences, char character)
    {
        var result = sentences.SelectMany(s => new LongSentenceSplitter(s, 100).SplitSentenceBy(character));

        Debug.WriteLine("SplitLongSentencesBy(): " + result.Count());

        return result;
    }

    public void GenerateWordViewsForWord(IEnumerable<(string contextWord, WordInContext studiedWord)> mappedWords)
    {
        foreach (var item in mappedWords)
        {
            if (item.studiedWord == null)
            {
                Button label = new()
                {
                    Text = item.contextWord,
                    HeightRequest = 50,
                };

                Body.Add(label);
            }
            else
            {
                WordEntryView view = new(item.studiedWord)
                {
                    HeightRequest = 50
                };

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
        Body.Add(new Button()
        {
            Text = "No words to repeat! 🙂",
            HeightRequest = 100
        });
    }

    public void Clear() => Body.Clear();
}
