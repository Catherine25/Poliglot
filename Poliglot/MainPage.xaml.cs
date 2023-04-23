using CommunityToolkit.Maui.Storage;
using Poliglot.Source.Extensions;
using Poliglot.Source.Statistics;
using Poliglot.Source.Storage;
using Poliglot.Source.Text;

namespace Poliglot;

public partial class MainPage : ContentPage
{
    private const string WordsFileName = "Words.json";
    private const string BlockedFileName = "Blocked.json";
    private const string StatisticsFileName = "Statistics.json";

    private WordBank wordBank;
    private BlockedBank blockedBank;
    private StatisticsBank statisticsBank;

    private Loader loader;
    private Saver saver;
    private TextProcessor textProcessor;
    private WordImporter wordImporter;

    public MainPage(IFileSaver fileSaver)
	{
		InitializeComponent();

        // todo DI
        SerializationOptions options = new();

        this.saver = new(fileSaver, options);
        this.loader = new(options);
        this.textProcessor = new();
        this.wordImporter = new(loader, textProcessor);

        WordStack.WordCompleted += WordStack_WordCompleted;

        Appearing += MainPage_AppearingAsync;

        ImportButton.Clicked += ImportButton_Clicked;
        SaveProgressButton.Clicked += SaveProgressButton_Clicked;
        AddNoteEntry.Completed += AddNoteEntry_Completed;
        BlockWordButton.Clicked += BlockWordButton_Clicked;
        BlockSentenceButton.Clicked += BlockSentenceButton_Clicked;
    }

    private void AddNoteEntry_Completed(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(AddNoteEntry.Text))
            return;

        wordBank.AddNote(WordStack.Word, AddNoteEntry.Text);
    }

    private void BlockWordButton_Clicked(object sender, EventArgs e)
    {
        string word = WordStack.Word.Original;

        wordBank.RemoveByWord(word);
        blockedBank.Words.Add(word);

        ShowNextWord();
    }

    private void BlockSentenceButton_Clicked(object sender, EventArgs e)
    {
        string sentence = WordStack.Word.Context;

        wordBank.RemoveBySentence(sentence);
        blockedBank.Sentenses.Add(sentence);

        ShowNextWord();
    }

    private void WordStack_WordCompleted(bool completed)
    {
        wordBank.CompleteWord(WordStack.Word, completed);
        StatisticsLabel.Text = statisticsBank.WordCompleted().ToString();

        ShowNextWord();
    }

    private void SaveProgressButton_Clicked(object sender, EventArgs e)
    {
        saver.Save(WordsFileName, wordBank);
        saver.Save(BlockedFileName, blockedBank);
        saver.Save(StatisticsFileName, statisticsBank);
    }

    private async void MainPage_AppearingAsync(object sender, EventArgs e)
    {
        wordBank = await loader.Load<WordBank>(WordsFileName);
        blockedBank = await loader.Load<BlockedBank>(BlockedFileName);
        statisticsBank = await loader.Load<StatisticsBank>(StatisticsFileName);

        ShowNextWord();
    }

    private async void ImportButton_Clicked(object sender, EventArgs e)
    {
        wordBank = await wordImporter.ImportInto(wordBank, blockedBank);

        ShowNextWord();
    }

    private void ShowNextWord()
    {
        if (!wordBank.Words.Any())
        {
            WordsAvailableBt.Text = "0";
            WordStack.ShowNoWords();
            return;
        }

        var wordsReadyForRepeating = wordBank.Words
            .Where(w => w.ReadyToForRepeating()) // word can be studied
            .OrderByDescending(w => w.State) // first repeat most known
            .OrderBy(w => w.Context != WordStack.Word?.Context); // show words with different sentence from the previous one to not spoil the word to the user

        if (!wordsReadyForRepeating.Any())
        {
            WordsAvailableBt.Text = "0";
            WordStack.ShowNoWords();
            return;
        }

        WordsAvailableBt.Text = wordsReadyForRepeating.Count().ToString();

        var word = wordsReadyForRepeating.SelectRandom();

        WordProgress.State = word.State;
        WordStack.Word = word;
        AddNoteEntry.Text = word.Note;
        StatisticsLabel.Text = statisticsBank.TodayEntry().WordCount.ToString();
    }
}

