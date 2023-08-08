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
        var sut = new ProofOfConceptParser();

        var actual = sut.TryParse("-l -p 8080");

        Assert.Equal(Validated.Succeed<string[], (bool, int)>((true, 8080)), actual);
    }
}
