using Poliglot.Source.Text;

namespace Poliglot;

public partial class TopMenu : ContentView
{
	public Action RequestDisplayAlert;

	public TopMenu()
	{
		InitializeComponent();

		SettingsBt.Clicked += SettingsBt_Clicked;
	}

    public void SetStatus(States state)
	{
        DisableAll();

        switch (state)
		{
			case States.New:
            {
		        New.IsEnabled = true;
			}
			break;
            case States.Seen:
			{
				Seen.IsEnabled = true;
			}
            break;
            case States.Studying:
			{
				Studying.IsEnabled = true;
			}
            break;
            case States.Recognized:
			{
				Recognized.IsEnabled = true;
			}
			break;
            case States.Known:
			{
				Known.IsEnabled = true;
			}
            break;
        }
	}

	private void DisableAll()
	{
        New.IsEnabled = false;
        Seen.IsEnabled = false;
        Studying.IsEnabled = false;
        Recognized.IsEnabled = false;
        Known.IsEnabled = false;
    }

    private void SettingsBt_Clicked(object sender, EventArgs e)
    {
        RequestDisplayAlert();
    }
}