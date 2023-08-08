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
        var idx = candidate.IndexOf($"-{flagName}");
        if (idx < 0)
            return Validated.Succeed<string, bool>(false);

        var nextFlagIdx = candidate.Substring(idx + 2).IndexOf('-');
        var bFlag = nextFlagIdx < 0
            ? candidate.Substring(idx + 2)
            : candidate.Substring(idx + 2, nextFlagIdx);
        if (bool.TryParse(bFlag, out var b))
            return Validated.Succeed<string, bool>(b);

        return Validated.Succeed<string, bool>(true);

    }
}