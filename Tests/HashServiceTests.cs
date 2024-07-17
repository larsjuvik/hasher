using System.Text;
using Services;
using Services.Models;

namespace Tests;

public class HashServiceTest
{
    // Test that the MD5 hash is correct
    [Theory]
    [InlineData("MD5", "test", "098f6bcd4621d373cade4e832627b4f6")]
    [InlineData("SHA1", "test", "a94a8fe5ccb19ba61c4c0873d391e987982fbbd3")]
    [InlineData("SHA256", "test", "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08")]
    [InlineData("SHA384", "test", "768412320f7b0aa5812fce428dc4706b3cae50e02a64caa16a782249bfe8efc4b7ef1ccb126255d196047dfedf17a0a9")]
    [InlineData("SHA512", "test", "ee26b0dd4af7e749aa1a8ee3c10ae9923f618980772e473f8819a5d4940e0db27ac185f8a0e1d5f84f88bc887fd67b143732c304cc5fa9ad8e6f57f50028a8ff")]
    public async Task TestHashesFromMemoryStream(string algorithm, string input, string expected)
    {
        var algorithmEnum = Enum.Parse<HashService.Algorithm>(algorithm);
        var inputMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        var progress = new Progress<HashingProgress>();

        var actual = await HashService.Hash(algorithmEnum, inputMemoryStream, progress, CancellationToken.None);

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

        await HashService.Hash(HashService.Algorithm.MD5, input, progress, CancellationToken.None);

        Assert.Equal(expected, actual);
    }
}