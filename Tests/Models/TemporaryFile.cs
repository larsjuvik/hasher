namespace Tests.Models;

using System;
using System.IO;

internal class TemporaryFile : IDisposable
{
    private readonly string _filePath;
    public FileStream FileStream { get; }

    public TemporaryFile(string input)
    {
        _filePath = Path.GetTempFileName();
        File.WriteAllText(_filePath, input);
        FileStream = File.OpenRead(_filePath);
    }

    public void Dispose()
    {
        FileStream.Dispose();
        File.Delete(_filePath);
    }
}