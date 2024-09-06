namespace Services;

using System.IO;
using Models;
using System.Security.Cryptography;

public static class HashService
{
    public enum Algorithm
    {
        Md5,
        Sha1,
        Sha256,
        Sha384,
        Sha512
    }

    public static string[] AvailableHashAlgorithms => Enum.GetNames(typeof(Algorithm));

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
            Algorithm.Md5 => MD5.Create(),
            Algorithm.Sha1 => SHA1.Create(),
            Algorithm.Sha256 => SHA256.Create(),
            Algorithm.Sha384 => SHA384.Create(),
            Algorithm.Sha512 => SHA512.Create(),
            _ => throw new Exception("Invalid algorithm")
        };
    }
}
