namespace Hasher.Viewmodels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services;
using Services.Models;

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
    private string[] hashAlgorithms = HashService.AvailableHashAlgorithms;

    [ObservableProperty]
    private string selectedHashAlgorithm = HashService.AvailableHashAlgorithms[0];

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

        // Convert chosen algorithm to enum
        var successParseAlgorithm = Enum.TryParse<HashService.Algorithm>(SelectedHashAlgorithm, false, out var algorithm);
        if (!successParseAlgorithm)
        {
            return;
        }

        Hash = await HashService.Hash(algorithm, fileStream, progress, cancellationToken);
    }
}