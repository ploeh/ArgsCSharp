using Xunit;

namespace Ploeh.Katas.ArgsCSharp;

public sealed class StringParserTests
{
    [Theory]
    [InlineData('g', "-g foo", "foo")]
    [InlineData('d', "-l -p 8080 -d /usr/logs", "/usr/logs")]
    [InlineData('d', "-p 8080 -d /usr/logs -l", "/usr/logs")]
    [InlineData('n', "-n fadsa -m gury", "fadsa")]
    public void TryParseSuccess(char flagName, string candidate, string expected)
    {
        var sut = new StringParser(flagName);
        var actual = sut.TryParse(candidate);
        Assert.Equal(Validated.Succeed<string, string>(expected), actual);
    }

    [Theory]
    [InlineData('t', "")]
    [InlineData('q', "")]
    [InlineData('f', "-l -p 8080 -d /usr/logs")]
    public void TryParseMissing(char flagName, string candidate)
    {
        var sut = new StringParser(flagName);
        var actual = sut.TryParse(candidate);
        Assert.Equal(
            Validated.Fail<string, string>($"Missing value for flag '-{flagName}'."),
            actual);
    }
}
