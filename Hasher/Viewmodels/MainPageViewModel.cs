namespace Hasher.Viewmodels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string selectedFilePath = "No file selected";

    [ObservableProperty]
    private string hash = null;

    [ObservableProperty]
    private bool isHashing = false;

    [ObservableProperty]
    private bool hasSelectedFile;

    [ObservableProperty]
    private string[] hashAlgorithms = HashService.HashAlgorithms;

    [ObservableProperty]
    private string selectedHashAlgorithm = HashService.HashAlgorithms[0];

    [RelayCommand]
    private async Task StartHashing()
    {
        throw new NotImplementedException();
    }
}