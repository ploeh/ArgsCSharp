using Xunit;

namespace Ploeh.Katas.ArgsCSharp;

public sealed class BoolParserTests
{
    [Fact]
    public void ParseSuccess()
    {
        var sut = new BoolParser("l");
        var actual = sut.Parse("-l");
        Assert.Equal(Validated.Succeed<string, bool>(true), actual);
    }
}
