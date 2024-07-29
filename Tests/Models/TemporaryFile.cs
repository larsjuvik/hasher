namespace Tests.Models;

using System;
using System.IO;

class TemporaryFile : IDisposable
{
    public string FilePath { get; }
    public FileStream FileStream { get; set; }

    public TemporaryFile(string input)
    {
        FilePath = Path.GetTempFileName();
        File.WriteAllText(FilePath, input);
        FileStream = File.OpenRead(FilePath);
    }

    public void Dispose()
    {
        FileStream.Dispose();
        File.Delete(FilePath);
    }
}