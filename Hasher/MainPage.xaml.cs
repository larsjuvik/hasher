using Hasher.Viewmodels;

namespace Hasher;

public partial class MainPage : ContentPage
{
	private MainPageViewModel _viewModel => (MainPageViewModel)BindingContext;


	public MainPage()
	{
		InitializeComponent();
		HashAlgorithmPicker.SelectedIndex = 0;
	}

	private async void CopyHashButton_Clicked(object sender, EventArgs e)
	{
		await Clipboard.SetTextAsync(_viewModel.Hash);
	}

	private async void SelectFileButton_Clicked(object sender, EventArgs e)
	{
		// Open file picker
		var filePickerResult = await FilePicker.Default.PickAsync(PickOptions.Default);

		// Create a file stream from result
		if (filePickerResult != null)
		{
			_viewModel.SelectedFilePath = filePickerResult.FullPath;
			_viewModel.HasSelectedFile = true;
			_viewModel.FilePickerResult = filePickerResult;
		}
		else
		{
			_viewModel.SelectedFilePath = "No valid file selected";
			_viewModel.HasSelectedFile = false;
		}
	}

	private async void VerifyHashButton_Clicked(object sender, EventArgs e)
	{
		if (_viewModel.Hash == null)
		{
			return;
		}

		var result = _viewModel.Hash == HashToVerifyEntry.Text;
		await DisplayAlert("Hash Verification", result ? "Hashes match!" : "Hashes do not match!", "OK");
	}
}

