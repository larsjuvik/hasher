namespace Hasher.Services;

using System.IO;
using Hasher.Services.Models;
using System.Security.Cryptography;

public static class HashService
{
    public enum HashAlgorithms
    {
        MD5
    }

    public static string[] AvailableHashAlgorithms { get => Enum.GetNames(typeof(HashAlgorithms)); }

    /// <summary>
    /// Hashes the input using the specified algorithm. Provides feedback on progress.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="progress"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<string> Hash(HashAlgorithms alg, Stream input, IProgress<HashingProgress> progress, CancellationToken cancellationToken)
    {
        using var hashAlgorithm = GetHashAlgorithm(alg);
        var buffer = new byte[8192];
        var bytesRead = 0;
        var totalBytes = input.Length;

        while ((bytesRead = await input.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
        {
            hashAlgorithm.TransformBlock(buffer, 0, bytesRead, null, 0);
            progress.Report(new HashingProgress { PercentageComplete = (float)input.Position / totalBytes });
        }
        hashAlgorithm.TransformFinalBlock(buffer, 0, 0);
        return BitConverter.ToString(hashAlgorithm.Hash).Replace("-", "").ToLower();
    }

    private static HashAlgorithm GetHashAlgorithm(HashAlgorithms alg)
    {
        return alg switch
        {
            HashAlgorithms.MD5 => System.Security.Cryptography.MD5.Create(),
            _ => throw new NotImplementedException()
        };
    }
}
