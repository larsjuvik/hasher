namespace Hasher.Viewmodels;

using CommunityToolkit.Mvvm.ComponentModel;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string selectedFileName = string.Empty;

    [ObservableProperty]
    private string hashAlgorithm = string.Empty;

    [ObservableProperty]
    private string hash = string.Empty;

    [ObservableProperty]
    private bool hasSelectedFile;

    [ObservableProperty]
    private string[] hashAlgorithms = HashService.HashAlgorithms;

    [ObservableProperty]
    private string selectedHashAlgorithm = HashService.HashAlgorithms[0];
}