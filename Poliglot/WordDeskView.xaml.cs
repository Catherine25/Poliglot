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

        var sentenceParts = ProduceWords(word.Context, word.Original);

        var mappedWords = sentenceParts
            .Select(w => (w.Value, w.Value == word.Original ? word : null));

        Body.Clear();

        GenerateWordViewsForWord(mappedWords);
    }

    private IEnumerable<MyMatch> ProduceWords(string text, string studiedWord)
    {
        var regex = new Regex("[a-zA-ZěĚšŠčČřŘžŽýÝáÁíÍéÉúÚůŮďĎťŤ]*");
        MatchCollection matchCollection = regex.Matches(text);

        var studiedMatches = matchCollection.Where(m => m.Value == studiedWord)
            .Select(m => new MyMatch(m.Index, m.Length, m.Value)).ToList();

        var resultingMatches = studiedMatches.Where(m => m.Value == studiedWord).ToList();

        for (int i = 0; i < studiedMatches.Count() - 1; i++)
        {
            var result = AddBetween(text, studiedMatches[i], studiedMatches[i + 1]);
            resultingMatches.AddRange(result);
        }

        resultingMatches = PrependFirst(studiedMatches, text).ToList();
        resultingMatches = AppendLast(text, studiedMatches).ToList();

        IEnumerable<string> extractedWords = matchCollection.Select(w => w.Value);

        Debug.WriteLine($"extracted words: {string.Join(' ', extractedWords)}");

        return resultingMatches;
    }

    private static IEnumerable<MyMatch> AddBetween(string text, MyMatch match1, MyMatch match2)
    {
        if (match1.Index == match2.Index)
            return new List<MyMatch> { match1, match2 };

        int start = match1.Index + match1.Length;
        int end = match2.Index;

        return new List<MyMatch> { match1, match2, new MyMatch(start, end, text.Substring(start, end - start)) };
    }

    private static IEnumerable<MyMatch> AppendLast(string text, IEnumerable<MyMatch> matches)
    {
        var lastMatch = matches.Last();

        int newWordIndex = lastMatch.Index + lastMatch.Length;
        int newWordLength = text.Length - newWordIndex;

        if (newWordIndex != text.Length)
        {
            MyMatch match = new(
                newWordIndex,
                newWordLength,
                text.Substring(newWordIndex, newWordLength)
            );

            matches = matches.Append(match);
        }

        return matches;
    }

    private IEnumerable<MyMatch> PrependFirst(IEnumerable<MyMatch> matches, string text)
    {
        var firstMatch = matches.First();

        if (firstMatch.Index != 0)
        {
            MyMatch element = new(0, firstMatch.Index, text.Substring(0, firstMatch.Index));
            matches = matches.Prepend(element);
        }

        return matches;
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
                    BackgroundColor = Colors.BlueViolet.WithAlpha(0.5f)
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

public class MyMatch
{
    public MyMatch(int index, int length, string value)
    {
        Index = index;
        Length = length;
        Value = value;
    }

    public int Index { get; set; }
    public int Length { get; set; }
    public string Value { get; set; }
}