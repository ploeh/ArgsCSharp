using Xunit;

namespace Ploeh.Katas.ArgsCSharp;

public sealed class IntParserTests
{
    [Theory]
    [InlineData('p', "-l -p 8080 -d /usr/logs", 8080)]
    [InlineData('f', "-f 2112", 2112)]
    [InlineData('f', "-f 2112 -t 1337", 2112)]
    [InlineData('t', "-f 2112 -t 1337", 1337)]
    public void ParseSuccess(char flagName, string candidate, int expected)
    {
        var sut = new IntParser(flagName);
        var actual = sut.Parse(candidate);
        Assert.Equal(Validated.Succeed<string, int>(expected), actual);
    }
}
