namespace Poliglot;

public partial class StatusButtonView : ContentView
{
	private bool _isActive;
	public bool IsActive
	{
		get => _isActive;
		set
		{
			_isActive = value;
			ActiveBt.IsVisible = _isActive;
			PassiveBt.IsVisible = !_isActive;
		}
	}

	public StatusButtonView()
	{
		InitializeComponent();
	}

	public StatusButtonView(string text)
	{
		InitializeComponent();

		ActiveBt.Text = text;
		PassiveBt.Text = text;
	}
}