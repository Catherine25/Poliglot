using Poliglot.Source.Text;

namespace Poliglot;

public partial class WordEntryView : ContentView
{
    public Action<bool> Completed;

    private readonly WordInContext word;
    private bool? answered;
    private bool hadMistake = false;

    public WordEntryView()
	{
		InitializeComponent();
	}

    public WordEntryView(string word)
    {
        InitializeComponent();

        Body.IsEnabled = false;
        Body.Text = word;
    }

    public WordEntryView(WordInContext word)
    {
        this.word = word;

        InitializeComponent();

        Body.IsEnabled = true;

        Body.Completed += Body_Completed;
    }

    private void Body_Completed(object sender, EventArgs e)
    {
        if (answered == true)
        {
            Completed(!hadMistake);
        }
        else
        {
            var correct = string.Equals(word.Original, Body.Text, StringComparison.OrdinalIgnoreCase);

            if (!correct)
            {
                hadMistake = true;
                Body.Text = string.Empty;
                Body.Placeholder = word.Original;
            }

            answered = correct;

            Body.TextColor = correct
                ? Colors.DarkCyan
                : Colors.Coral;
        }
    }
}