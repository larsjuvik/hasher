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

	private async void SelectFileButton_Clicked(object sender, EventArgs e)
	{
		// Open file picker
		var filePickerResult = await FilePicker.Default.PickAsync(PickOptions.Default);

		// Create a file stream from result
		if (filePickerResult != null)
		{
			_viewModel.SelectedFilePath = filePickerResult.FullPath;
			_viewModel.HasSelectedFile = true;
		}
		else
		{
			_viewModel.ErrorMessage = "No file selected";
			_viewModel.HasErrorMessage = true;
			_viewModel.SelectedFilePath = "";
			_viewModel.HasSelectedFile = false;
		}
	}
}

