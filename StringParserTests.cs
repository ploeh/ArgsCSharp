using Xunit;

namespace Ploeh.Katas.ArgsCSharp;

public sealed class StringParserTests
{
    [Theory]
    [InlineData('g', "-g foo", "foo")]
    [InlineData('d', "-l -p 8080 -d /usr/logs", "/usr/logs")]
    [InlineData('d', "-p 8080 -d /usr/logs -l", "/usr/logs")]
    public void TryParseSuccess(char flagName, string candidate, string expected)
    {
        var sut = new StringParser(flagName);
        var actual = sut.TryParse(candidate);
        Assert.Equal(Validated.Succeed<string, string>(expected), actual);
    }
}
