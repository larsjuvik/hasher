using System.Text;
using Hasher.Services;
using Hasher.Services.Models;

namespace Hasher.Tests;

public class HashServiceTest
{
    // Test that the MD5 hash is correct
    [Fact]
    public async Task TestMD5HashFromMemoryStream()
    {
        var input = new MemoryStream(Encoding.UTF8.GetBytes("test"));
        var expected = "098f6bcd4621d373cade4e832627b4f6";
        var progress = new Progress<HashingProgress>();

        var actual = await HashService.Hash(HashService.HashAlgorithms.MD5, input, progress, CancellationToken.None);

        Assert.Equal(expected, actual);
    }

    // Test that the progress is correct
    [Fact]
    public async Task TestMD5ProgressFromMemoryStream()
    {
        var input = new MemoryStream(Encoding.UTF8.GetBytes("test"));
        var progress = new Progress<HashingProgress>();
        var expected = new List<float> { 1.0f };

        var actual = new List<float>();
        progress.ProgressChanged += (sender, e) => actual.Add(e.PercentageComplete);

        await HashService.Hash(HashService.HashAlgorithms.MD5, input, progress, CancellationToken.None);

        Assert.Equal(expected, actual);
    }
}