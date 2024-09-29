using System.ComponentModel;
using System.Diagnostics;

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

    /// <summary>
    /// Returns an algorithm from a string (ignoring case)
    /// </summary>
    /// <param name="algorithm">The algorithm to get</param>
    /// <returns>The algorithm enum with value corresponding to string</returns>
    public static Algorithm GetAlgorithmFromString(string algorithm)
    {
        return Enum.Parse<Algorithm>(algorithm, true);
    }

    public static string[] AvailableHashAlgorithms => Enum.GetNames(typeof(Algorithm)).Select(a => a.ToUpper()).ToArray();

    /// <summary>
    /// Hashes the input using the specified algorithm. Provides feedback on progress.
    /// </summary>
    /// <param name="algorithm">The algorithm to use for hashing</param>
    /// <param name="input">The input to the hasher</param>
    /// <param name="progress">A progress reporter</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The hash of the input</returns>
    /// <exception cref="NullReferenceException">Thrown if hash result is null</exception>
    /// <exception cref="InvalidEnumArgumentException">Thrown when an invalid algorithm is specified</exception>
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

        if (hashAlgorithm.Hash == null)
        {
            throw new NullReferenceException("Hash Algorithm is null");
        }

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
            _ => throw new InvalidEnumArgumentException("Invalid algorithm")
        };
    }
}
