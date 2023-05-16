using Poliglot.Source.Text;

namespace Poliglot;

public partial class WordProgressView : ContentView
{
    public States State
	{
		get => states;
		set
		{
			states = value;
			DisableAll();
			stateButtonMapping[states].IsActive = true;
        }
	}
	private States states;

	public WordProgressView()
	{
		InitializeComponent();

		stateButtonMapping = new Dictionary<States, StatusButtonView>
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
			button.IsActive = false;
    }

	private Dictionary<States, StatusButtonView> stateButtonMapping;
}