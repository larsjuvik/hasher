namespace Hasher;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

	private async void SelectFileButton_Clicked(object sender, EventArgs e)
	{
		// Open file picker
		var filePickerResult = await FilePicker.Default.PickAsync(PickOptions.Default);

		// Create a file stream from result
		if (filePickerResult != null)
		{
			var fileStream = await filePickerResult.OpenReadAsync();

			// Create a hash from the file stream
			var progress = new Progress<HashingProgress>();
			progress.ProgressChanged += (sender, e) => ProgressBar.Progress = e.PercentageComplete;
			var hash = await Hasher.HashService.MD5(fileStream, progress, CancellationToken.None);

			// Display the hash
			HashLabel.Text = hash;
		}
		else
		{
			HashLabel.Text = "No file selected";
		}
	}
}

