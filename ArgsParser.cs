namespace Ploeh.Katas.ArgsCSharp;

public sealed class ArgsParser<T1, T2, T>
{
    private readonly IParser<T1> parser1;
    private readonly IParser<T2> parser2;
    private readonly Func<T1, T2, T> create;

    public ArgsParser(IParser<T1> parser1, IParser<T2> parser2, Func<T1, T2, T> create)
    {
        this.parser1 = parser1;
        this.parser2 = parser2;
        this.create = create;
    }

    public Validated<string[], T> TryParse(string candidate)
    {
        var x1 = parser1.TryParse(candidate).SelectFailure(s => new[] { s });
        var x2 = parser2.TryParse(candidate).SelectFailure(s => new[] { s });
        return create.Apply(x1, CombineErrors).Apply(x2, CombineErrors);
    }

    private static string[] CombineErrors(string[] s1, string[] s2)
    {
        return s1.Concat(s2).ToArray();
    }
}
