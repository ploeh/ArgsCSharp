namespace Ploeh.Katas.ArgsCSharp;

public sealed class IntParser
{
    private readonly char flagName;

    public IntParser(char flagName)
    {
        this.flagName = flagName;
    }

    public Validated<string, int> TryParse(string candidate)
    {
        var idx = candidate.IndexOf($"-{flagName}");

        var nextFlagIdx = candidate[(idx + 2)..].IndexOf('-');
        var bFlag = nextFlagIdx < 0
            ? candidate[(idx + 2)..]
            : candidate.Substring(idx + 2, nextFlagIdx);
        if (int.TryParse(bFlag, out var i))
            return Validated.Succeed<string, int>(i);

        throw new NotImplementedException();
    }
}