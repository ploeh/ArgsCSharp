namespace Ploeh.Katas.ArgsCSharp;

public sealed class IntParser : IParser<int>
{
    private readonly char flagName;

    public IntParser(char flagName)
    {
        this.flagName = flagName;
    }

    public Validated<string, int> TryParse(string candidate)
    {
        var idx = candidate.IndexOf($"-{flagName}");
        if (idx < 0)
            return Validated.Fail<string, int>(
                $"Missing value for flag '-{flagName}'.");

        var nextFlagIdx = candidate[(idx + 2)..].IndexOf('-');
        string iFlag = FindValue(candidate, idx, nextFlagIdx);
        if (int.TryParse(iFlag, out var i))
            return Validated.Succeed<string, int>(i);

        return Validated.Fail<string, int>(
            $"""Expected integer for flag '-{flagName}', but got "{iFlag}".""");
    }

    private static string FindValue(string candidate, int idx, int nextFlagIdx)
    {
        if (nextFlagIdx < 0)
            return candidate[(idx + 2)..].Trim();

        return candidate.Substring(idx + 2, nextFlagIdx).Trim();
    }
}