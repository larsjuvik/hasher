using Hasher.Viewmodels;

namespace Hasher;

public partial class App
{
	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		MainPage = new MainPage(serviceProvider.GetRequiredService<MainPageViewModel>());
	}
}
