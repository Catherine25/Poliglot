using CommunityToolkit.Maui.Storage;
using Poliglot.Source.Database;
using Poliglot.Source.Database.Base;
using Poliglot.Source.Extensions;
using Poliglot.Source.Storage;
using Poliglot.Source.Text;

namespace Poliglot;

public partial class MainPage : ContentPage
{
    private Loader loader;
    private TextProcessor textProcessor;
    private WordImporter wordImporter;

    private readonly PoliglotDatabase poliglotDatabase;

    public MainPage(IFileSaver fileSaver, PoliglotDatabase poliglotDatabase)
	{
		InitializeComponent();

        // todo DI
        SerializationOptions options = new();

        this.loader = new(options);
        this.textProcessor = new();
        this.wordImporter = new(loader, textProcessor);

        this.poliglotDatabase = poliglotDatabase;

        WordStack.WordCompleted += WordStack_WordCompleted;

        Appearing += MainPage_AppearingAsync;

        ImportButton.Clicked += ImportButton_Clicked;
        SentenceTransationEntry.Completed += SentenceTransationEntry_Completed;
        NoteEntry.Completed += NoteEntry_Completed;
        BlockWordButton.Clicked += BlockWordButton_Clicked;
        BlockSentenceButton.Clicked += BlockSentenceButton_Clicked;
    }

    private void SentenceTransationEntry_Completed(object sender, EventArgs e)
    {
        var text = SentenceTransationEntry.Text;

        if (string.IsNullOrEmpty(text))
            return;

        WordStack.Word.SentenceNote = text;
    }

    private async void NoteEntry_Completed(object sender, EventArgs e)
    {
        var text = NoteEntry.Text;

        if (string.IsNullOrEmpty(text))
            return;

        WordStack.Word.Note = text;
    }

    private void BlockWordButton_Clicked(object sender, EventArgs e)
    {
        WordStack.Word.WordBlocked = true;
        ShowNextWord();
    }

    private void BlockSentenceButton_Clicked(object sender, EventArgs e)
    {
        WordStack.Word.SentenceBlocked = true;
        ShowNextWord();
    }

    private void WordStack_WordCompleted(bool completed)
    {
        WordStack.Word.Answered(completed);

        //StatisticsLabel.Text = statisticsBank.WordCompleted().ToString();

        ShowNextWord();
    }

    private async void MainPage_AppearingAsync(object sender, EventArgs e)
    {
        ShowNextWord();
    }

    // todo
    private async void ImportButton_Clicked(object sender, EventArgs e)
    {
        var oldWords = await poliglotDatabase.GetItemsAsync<WordDbItem>();
        var oldSentences = await poliglotDatabase.GetItemsAsync<SentenceDbItem>();

        var wordsInContext = await wordImporter.ImportInto(oldWords, oldSentences); //wordBank, blockedBank);
        var itemsInContext = new List<WordInContext>();

        // save sentences
        foreach (var wordInContext in wordsInContext)
        {
            // extract sentence if
            var sentenceItem = await poliglotDatabase.GetItemAsync<SentenceDbItem>(x => x.Sentence == wordInContext.sentence);

            if (sentenceItem == null)
            {
                sentenceItem = new()
                {
                    Sentence = wordInContext.sentence
                };

                await poliglotDatabase.SaveItemAsync(sentenceItem);
            }

            var wordItem = new WordDbItem()
            {
                Word = wordInContext.word,
                SentenceId = sentenceItem.Id,
                State = States.New,
            };

            await poliglotDatabase.SaveItemAsync(wordItem);
        }

        ShowNextWord();
    }

    private async void ShowNextWord()
    {
        if (! await poliglotDatabase.Any<WordDbItem>())
        {
            WordsAvailableBt.Text = "0";
            WordStack.ShowNoWords();
            return;
        }

        if(WordStack.Word != null)
        {
            var wordDbItem = WordStack.Word.wordDbItem;
            if(wordDbItem != null)
                poliglotDatabase.SaveItemAsync(wordDbItem);

            var sentenceDbItem = WordStack.Word.sentenceDbItem;
            if(sentenceDbItem != null)
                poliglotDatabase.SaveItemAsync(sentenceDbItem);
        }

        var wordsReadyForRepeating = await poliglotDatabase.GetWordsReadyForRepeating(WordStack.Word?.Context);

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

        SentenceTransationEntry.Text = word.SentenceNote;
        NoteEntry.Text = word.Note;
        //StatisticsLabel.Text = statisticsBank.TodayEntry().WordCount.ToString();
    }
}

