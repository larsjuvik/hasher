using Hasher.GuiServices.Interfaces;

namespace Hasher.GuiServices;

public class DisplayService : IDisplayService
{
    public async Task ShowAsync(string title, string message)
    {
        if (Application.Current?.MainPage is not null)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }
    }
}