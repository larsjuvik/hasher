using Hasher.Viewmodels;

namespace Hasher;

public partial class MainPage
{
	private MainPageViewModel ViewModel => (MainPageViewModel)BindingContext;
	
	public MainPage()
	{
		InitializeComponent();
		HashAlgorithmPicker.SelectedIndex = 0;
	}

	private async void CopyHashButton_Clicked(object sender, EventArgs e)
	{
		await Clipboard.SetTextAsync(ViewModel.Hash);
	}

	private async void SelectFileButton_Clicked(object sender, EventArgs e)
	{
		// Open file picker
		var filePickerResult = await FilePicker.Default.PickAsync(PickOptions.Default);

		// Create a file stream from result
		if (filePickerResult != null)
		{
			ViewModel.SelectedFilePath = filePickerResult.FullPath;
			ViewModel.HasSelectedFile = true;
			ViewModel.FilePickerResult = filePickerResult;
		}
		else
		{
			ViewModel.SelectedFilePath = "No valid file selected";
			ViewModel.HasSelectedFile = false;
		}
	}

	private async void VerifyHashButton_Clicked(object sender, EventArgs e)
	{
		if (ViewModel.Hash == null)
		{
			return;
		}

		var result = ViewModel.Hash == HashToVerifyEntry.Text;
		await DisplayAlert("Hash Verification", result ? "Hashes match!" : "Hashes do not match!", "OK");
	}
}

