namespace Hasher.Viewmodels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hasher;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string selectedFilePath = "No file selected";

    [ObservableProperty]
    private FileResult? filePickerResult;

    [ObservableProperty]
    private string? hash = null;

    [ObservableProperty]
    private bool hasSelectedFile;

    [ObservableProperty]
    private string[] hashAlgorithms = HashService.HashAlgorithms;

    [ObservableProperty]
    private string selectedHashAlgorithm = HashService.HashAlgorithms[0];

    [ObservableProperty]
    private float hashingProgress = 0.0f;

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
        switch (SelectedHashAlgorithm)
        {
            case "MD5":
                Hash = await HashService.MD5(fileStream, progress, cancellationToken);
                break;
            default:
                return;
        }

    }
}