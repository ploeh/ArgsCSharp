using Xunit;

namespace Ploeh.Katas.ArgsCSharp;

public sealed class IntParserTests
{
    [Theory]
    [InlineData('p', "-l -p 8080 -d /usr/logs", 8080)]
    [InlineData('f', "-f 2112", 2112)]
    [InlineData('f', "-f 2112 -t 1337", 2112)]
    [InlineData('t', "-f 2112 -t 1337", 1337)]
    public void TryParseSuccess(char flagName, string candidate, int expected)
    {
        var sut = new IntParser(flagName);
        var actual = sut.TryParse(candidate);
        Assert.Equal(Validated.Succeed<string, int>(expected), actual);
    }

    [Theory]
    [InlineData('t', "")]
    [InlineData('q', "")]
    [InlineData('f', "-l -p 8080 -d /usr/logs")]
    public void TryParseMissing(char flagName, string candidate)
    {
        var sut = new IntParser(flagName);
        var actual = sut.TryParse(candidate);
        Assert.Equal(
            Validated.Fail<string, int>($"Missing value for flag '-{flagName}'."),
            actual);
    }

    [Theory]
    [InlineData('l', "-l fadfdcip", "fadfdcip")]
    [InlineData('g', "-g hnv14", "hnv14")]
    [InlineData('p', "-l -p 80dt -d /usr/logs", "80dt")]
    public void TryParseMalformed(char flagName, string candidate, string expected)
    {
        var sut = new IntParser(flagName);
        var actual = sut.TryParse(candidate);
        Assert.Equal(
            Validated.Fail<string, int>(
                $"""Expected integer for flag '-{flagName}', but got "{expected}"."""),
            actual);
    }
}
