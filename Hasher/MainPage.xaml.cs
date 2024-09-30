using Hasher.Viewmodels;

namespace Hasher;

public partial class MainPage
{
	private MainPageViewModel ViewModel => (MainPageViewModel)BindingContext;
	
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		HashAlgorithmPicker.SelectedIndex = 0;
	}

	private async void CopyHashButton_Clicked(object sender, EventArgs e)
	{
		await Clipboard.SetTextAsync(ViewModel.Hash);
	}
}