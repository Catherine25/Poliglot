using CommunityToolkit.Maui.Storage;
using Poliglot.Source.Extensions;
using Poliglot.Source.Storage;
using Poliglot.Source.Text;

namespace Poliglot;

public partial class MainPage : ContentPage
{
    private const string WordsFileName = "Words.json";
    private const string BlockedFileName = "Blocked.json";

    private WordBank wordBank;
    private BlockedBank blockedBank;

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
        wordBank.RemoveByWord(WordStack.Word.Original);

        ShowNextWord();
    }

    private void BlockSentenceButton_Clicked(object sender, EventArgs e)
    {
        wordBank.RemoveBySentence(WordStack.Word.Context);

        ShowNextWord();
    }

    private void WordStack_WordCompleted(bool completed)
    {
        wordBank.CompleteWord(WordStack.Word, completed);

        ShowNextWord();
    }

    private void SaveProgressButton_Clicked(object sender, EventArgs e)
    {
        saver.Save(WordsFileName, wordBank);
    }

    private async void MainPage_AppearingAsync(object sender, EventArgs e)
    {
        // load known words
        wordBank = await loader.Load<WordBank>(WordsFileName);
        blockedBank = await loader.Load<BlockedBank>(BlockedFileName);

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
            WordStack.ShowNoWords();
            return;
        }

        var word = wordBank.Words
            .Where(w => w.ReadyToForRepeating()) // word can be studied
            .Where(w => w.Context != WordStack.Word?.Context) // sentence is not the same
            .SelectRandom();

        WordStack.Word = word;
        AddNoteEntry.Text = word.Note;
    }
}

