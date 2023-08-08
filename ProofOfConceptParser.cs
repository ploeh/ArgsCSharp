namespace Ploeh.Katas.ArgsCSharp;

internal sealed class ProofOfConceptParser : ArgsParser<bool, int, (bool, int)>
{
    public ProofOfConceptParser() : base(new BoolParser('l'), new IntParser('p'))
    {
    }

    protected override (bool, int) Create(bool x1, int x2)
    {
        return (x1, x2);
    }
}
