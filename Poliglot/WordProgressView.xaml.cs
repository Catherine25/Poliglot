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

		stateButtonMapping = new Dictionary<States, StatusButtonView>();

		CreateButton(States.New, "New");
		CreateButton(States.Seen, "Seen");
		CreateButton(States.Studying, "Studying");
		CreateButton(States.Recognized, "Recognized");
		CreateButton(States.Known, "Known");
    }

	private void CreateButton(States state, string content)
    {
        var newBt = new StatusButtonView(content);
        Stack.Add(newBt);
        stateButtonMapping.Add(state, newBt);
    }

	private void DisableAll()
	{
		foreach (var button in stateButtonMapping.Values)
			button.IsActive = false;
    }

	private Dictionary<States, StatusButtonView> stateButtonMapping;
}