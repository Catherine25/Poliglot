using Poliglot.Source.Text;

namespace Poliglot;

public partial class WordProgressView : ContentView
{
	private readonly SolidColorBrush disabledBrush = new(Colors.Gray);
	private readonly SolidColorBrush enabledBrush = new(Colors.Red);

    public States State
	{
		get => states;
		set
		{
			states = value;
			DisableAll();
			stateButtonMapping[states].Background = enabledBrush;
        }
	}
	private States states;

	public WordProgressView()
	{
		InitializeComponent();

		stateButtonMapping = new Dictionary<States, Button>
		{
			{ States.New, NewBt },
			{ States.Seen, SeenBt },
			{ States.Studying, StudyingBt },
			{ States.Recognized, RecognizedBt },
			{ States.Known, KnownBt },
		};
	}

	private void DisableAll()
	{
		foreach (var button in stateButtonMapping.Values)
			button.Background = disabledBrush;
    }

	private Dictionary<States, Button> stateButtonMapping;
}