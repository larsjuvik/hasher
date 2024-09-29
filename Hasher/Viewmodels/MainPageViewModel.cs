namespace Hasher.Viewmodels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services;
using Services.Models;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string _selectedFilePath = "No file selected";

    [ObservableProperty]
    private FileResult? _filePickerResult;
    
    [ObservableProperty]
    private string? _errorMessage = null;

    [ObservableProperty]
    private string? _hash = null;
    
    [ObservableProperty]
    private string? _verifyText = null;

    [ObservableProperty]
    private bool _hasSelectedFile;

    [ObservableProperty]
    private string[] _hashAlgorithms = HashService.AvailableHashAlgorithms;

    [ObservableProperty]
    private string _selectedHashAlgorithm = HashService.AvailableHashAlgorithms[0];

    [ObservableProperty]
    private float _hashingProgress = 0.0f;
    
    [RelayCommand]
    private async Task SelectFile()
    {
        // Open file picker
        var filePickerResult = await FilePicker.Default.PickAsync(PickOptions.Default);

        // Create a file stream from result
        if (filePickerResult != null)
        {
            SelectedFilePath = filePickerResult.FullPath;
            HasSelectedFile = true;
            FilePickerResult = filePickerResult;
        }
        else
        {
            SelectedFilePath = "No valid file selected";
            HasSelectedFile = false;
        }
    }

    [RelayCommand]
    private async Task VerifyHash()
    {
        if (Hash == null)
        {
            return;
        }

        var result = Hash == VerifyText;
        await Application.Current.MainPage.DisplayAlert("Hash Verification", result ? "Hashes match!" : "Hashes do not match!", "OK");
    }

    [RelayCommand]
    private async Task StartHashing()
    {
        if (!HasSelectedFile || FilePickerResult == null)
        {
            return;
        }

        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        var progress = new Progress<HashingProgress>(p =>
        {
            HashingProgress = p.PercentageComplete;
        });

        var fileStream = await FilePickerResult.OpenReadAsync();

        // Convert chosen algorithm to enum
        try
        {
            var algorithm = HashService.GetAlgorithmFromString(SelectedHashAlgorithm);
            Hash = await HashService.Hash(algorithm, fileStream, progress, cancellationToken);
        }
        catch (Exception)
        {
            ErrorMessage = "Hashing failed";
        }
    }
}