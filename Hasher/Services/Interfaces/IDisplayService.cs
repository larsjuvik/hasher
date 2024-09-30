namespace Hasher.Services;

public interface IDisplayService
{
    public Task ShowAsync(string title, string message);
}