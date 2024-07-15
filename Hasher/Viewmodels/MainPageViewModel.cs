namespace Hasher.Viewmodels;

using CommunityToolkit.Mvvm.ComponentModel;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string selectedFileName;

    [ObservableProperty]
    private string hashAlgorithm;

    [ObservableProperty]
    private string hash;

    [ObservableProperty]
    private bool hasSelectedFile;

}