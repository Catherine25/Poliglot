using Poliglot.Source.Text;

namespace Poliglot;

public partial class WordEntryView : ContentView
{
    public Action<bool> Completed;

    private readonly WordInContext word;
    private bool? answered;
    private bool hadMistake = false;

    Animation correctAnimation;
    Animation incorrectAnimation;

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

        correctAnimation = new Animation(v => Body.BackgroundColor = Colors.LightCyan, 1, 2);
        incorrectAnimation = new Animation(v => Body.BackgroundColor = Colors.LightCoral, 1, 2);
    }

    private void Body_Completed(object sender, EventArgs e)
    {
        if (answered == true)
        {
            Completed(!hadMistake);
        }
        else
        {
            var correct = string.Equals(word.Original, (sender as Entry).Text, StringComparison.OrdinalIgnoreCase);

            if (!correct)
            {
                hadMistake = true;
                Body.Text = string.Empty;
                Body.Placeholder = word.Original;
            }

            answered = correct;

            if(correct)
                correctAnimation.Commit(this, "SimpleAnimation", 16, 6000);
            else
                incorrectAnimation.Commit(this, "SimpleAnimation", 16, 6000);
        }
    }
}