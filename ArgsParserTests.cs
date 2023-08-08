using Xunit;

namespace Ploeh.Katas.ArgsCSharp;

public sealed class ArgsParserTests
{
    [Fact]
    public void ParseBoolAndIntProofOfConceptRaw()
    {
        var args = "-l -p 8080";
        var l = new BoolParser('l').TryParse(args).SelectFailure(s => new[] { s });
        var p = new IntParser('p').TryParse(args).SelectFailure(s => new[] { s });
        Func<bool, int, (bool, int)> createTuple = (b, i) => (b, i);
        static string[] combineErrors(string[] s1, string[] s2) => s1.Concat(s2).ToArray();

        var actual = createTuple.Apply(l, combineErrors).Apply(p, combineErrors);

        Assert.Equal(Validated.Succeed<string[], (bool, int)>((true, 8080)), actual);
    }

    [Fact]
    public void ParseBoolAndIntProofOfConcept()
    {
        var sut = new ArgsParser<bool, int, (bool, int)>(
            new BoolParser('l'),
            new IntParser('p'),
            (b, i) => (b, i));

        var actual = sut.TryParse("-l -p 8080");

        Assert.Equal(Validated.Succeed<string[], (bool, int)>((true, 8080)), actual);
    }

    private sealed record TestConfig(bool DoLog, int Port, string Directory);

    [Theory]
    [InlineData("-l -p 8080 -d /usr/logs")]
    [InlineData("-p 8080 -l -d /usr/logs")]
    [InlineData("-d /usr/logs -l -p 8080")]
    [InlineData(" -d  /usr/logs  -l  -p 8080  ")]
    public void ParseConfig(string args)
    {
        var sut = new ArgsParser<bool, int, string, TestConfig>(
            new BoolParser('l'),
            new IntParser('p'),
            new StringParser('d'),
            (b, i, s) => new TestConfig(b, i, s));

        var actual = sut.TryParse(args);

        Assert.Equal(
            Validated.Succeed<string[], TestConfig>(
                new TestConfig(true, 8080, "/usr/logs")),
            actual);
    }

    [Fact]
    public void FailToParseConfig()
    {
        var sut = new ArgsParser<bool, int, string, TestConfig>(
            new BoolParser('l'),
            new IntParser('p'),
            new StringParser('d'),
            (b, i, s) => new TestConfig(b, i, s));

        var actual = sut.TryParse("-p aityaity");

        Assert.True(actual.Match(
            onFailure: ss => ss.Contains("Expected integer for flag '-p', but got \"aityaity\"."),
            onSuccess: _ => false));
        Assert.True(actual.Match(
            onFailure: ss => ss.Contains("Missing value for flag '-d'."),
            onSuccess: _ => false));
    }
}
