namespace Ploeh.Katas.ArgsCSharp;

public abstract class ArgsParser<T1, T2, T>
{
    private readonly IParser<T1> parser1;
    private readonly IParser<T2> parser2;

    public ArgsParser(IParser<T1> parser1, IParser<T2> parser2)
    {
        this.parser1 = parser1;
        this.parser2 = parser2;
    }

    public Validated<string[], T> TryParse(string candidate)
    {
        var l = parser1.TryParse(candidate).SelectFailure(s => new[] { s });
        var p = parser2.TryParse(candidate).SelectFailure(s => new[] { s });

        Func<T1, T2, T> create = Create;
        return create.Apply(l, CombineErrors).Apply(p, CombineErrors);
    }

    protected abstract T Create(T1 x1, T2 x2);

    private static string[] CombineErrors(string[] s1, string[] s2)
    {
        return s1.Concat(s2).ToArray();
    }
}
