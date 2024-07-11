namespace Tests;

public class HashServiceTest
{
    // Test that the MD5 hash is correct
    [Fact]
    public void TestMD5()
    {
        var input = "Hello, World!";
        var expected = "65a8e27d8879283831b664bd8b7f0ad4";
        var actual = Hashing.HashService.MD5(input);
        Assert.Equal(expected, actual);
    }
}