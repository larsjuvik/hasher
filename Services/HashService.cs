namespace Hasher;

using System.IO;

public static class HashService
{
    public static readonly string[] HashAlgorithms = new string[]
    {
        "MD5"
    };

    /// <summary>
    /// Hashes the input using the specified algorithm. Provides feedback on progress.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="progress"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<string> MD5(Stream input, IProgress<HashingProgress> progress, CancellationToken cancellationToken)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        var buffer = new byte[8192];
        var bytesRead = 0;
        var totalBytes = input.Length;

        while ((bytesRead = await input.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
        {
            md5.TransformBlock(buffer, 0, bytesRead, null, 0);
            progress.Report(new HashingProgress { PercentageComplete = (float)input.Position / totalBytes });
        }
        md5.TransformFinalBlock(buffer, 0, 0);
        return BitConverter.ToString(md5.Hash).Replace("-", "").ToLower();
    }
}
