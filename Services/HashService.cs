namespace Services;

using System.IO;
using Services.Models;
using System.Security.Cryptography;

public static class HashService
{
    public enum Algorithm
    {
        MD5,

        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

    public static string[] AvailableHashAlgorithms { get => Enum.GetNames(typeof(Algorithm)); }

    /// <summary>
    /// Hashes the input using the specified algorithm. Provides feedback on progress.
    /// </summary>
    /// <param name="algorithm">The algorithm to use for hashing</param>
    /// <param name="input">The input to the hasher</param>
    /// <param name="progress">A progress reporter</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The hash of the input</returns>
    /// <exception cref="Exception">Thrown when an invalid algorithm is specified</exception>
    public static async Task<string> Hash(Algorithm algorithm, Stream input, IProgress<HashingProgress> progress, CancellationToken cancellationToken)
    {
        using var hashAlgorithm = GetHashAlgorithm(algorithm);
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

    private static HashAlgorithm GetHashAlgorithm(Algorithm algorithm)
    {
        return algorithm switch
        {
            Algorithm.MD5 => System.Security.Cryptography.MD5.Create(),
            Algorithm.SHA1 => System.Security.Cryptography.SHA1.Create(),
            Algorithm.SHA256 => System.Security.Cryptography.SHA256.Create(),
            Algorithm.SHA384 => System.Security.Cryptography.SHA384.Create(),
            Algorithm.SHA512 => System.Security.Cryptography.SHA512.Create(),
            _ => throw new Exception("Invalid algorithm")
        };
    }
}
