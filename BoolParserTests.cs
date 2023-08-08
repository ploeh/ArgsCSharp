using Xunit;

namespace Ploeh.Katas.ArgsCSharp;

public sealed class BoolParserTests
{
    [Theory]
    [InlineData('l', "-l", true)]
    [InlineData('l', " -l ", true)]
    [InlineData('l', "-l -p 8080 -d /usr/logs", true)]
    [InlineData('l', "-p 8080 -l -d /usr/logs", true)]
    [InlineData('l', "-p 8080 -d /usr/logs", false)]
    [InlineData('l', "-l true", true)]
    [InlineData('l', "-l false", false)]
    [InlineData('l', "nonsense", false)]
    [InlineData('f', "-f", true)]
    [InlineData('f', "foo", false)]
    [InlineData('f', "", false)]
    public void TryParseSuccess(char flagName, string candidate, bool expected)
    {
        var sut = new BoolParser(flagName);
        var actual = sut.TryParse(candidate);
        Assert.Equal(Validated.Succeed<string, bool>(expected), actual);
    }
}
