using CommunityToolkit.Maui;
using Hasher.GuiServices;
using Hasher.GuiServices.Interfaces;
using Hasher.Viewmodels;
using Microsoft.Extensions.Logging;

namespace Hasher;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		
		// Services
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddSingleton<IDisplayService, DisplayService>();
		
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
