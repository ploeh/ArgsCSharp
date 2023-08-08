namespace Ploeh.Katas.ArgsCSharp;

public sealed class BoolParser
{
    private readonly string flagName;

    public BoolParser(string flagName)
    {
        this.flagName = flagName;
    }

    public Validated<string, bool> Parse(string candidate)
    {
        return Validated<string, bool>.Succeed(true);
    }
}