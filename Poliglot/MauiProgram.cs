using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Logging;
using Poliglot.Source.Database.Base;

namespace Poliglot;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit();

		builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
		builder.Services.AddTransient<MainPage>();

        builder.Services.AddSingleton<PoliglotDatabase>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
