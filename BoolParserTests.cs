using Xunit;

namespace Ploeh.Katas.ArgsCSharp;

public sealed class BoolParserTests
{
    [Theory]
    [InlineData("-l", true)]
    [InlineData(" -l ", true)]
    [InlineData("-l -p 8080 -d /usr/logs", true)]
    [InlineData("-p 8080 -l -d /usr/logs", true)]
    [InlineData("-p 8080 -d /usr/logs", false)]
    [InlineData("-l true", true)]
    [InlineData("-l false", false)]
    [InlineData("nonsense", false)]
    public void ParseSuccess(string candidate, bool expected)
    {
        var sut = new BoolParser('l');
        var actual = sut.Parse(candidate);
        Assert.Equal(Validated.Succeed<string, bool>(expected), actual);
    }
}
