using Xunit;

namespace Ploeh.Katas.ArgsCSharp;

public sealed class ArgsParserTests
{
    [Fact]
    public void ParseBoolAndIntProofOfConcept()
    {
        var args = "-l -p 8080";
        var l = new BoolParser('l').Parse(args).SelectFailure(s => new[] { s });
        var p = new IntParser('p').Parse(args).SelectFailure(s => new[] { s });
        Func<bool, int, (bool, int)> createTuple = (b, i) => (b, i);
        Func<string[], string[], string[]> combineErrors = (s1, s2) => s1.Concat(s2).ToArray();

        var actual = createTuple.Apply(l, combineErrors).Apply(p, combineErrors);

        Assert.Equal(Validated.Succeed<string[], (bool, int)>((true, 8080)), actual);
    }
}
