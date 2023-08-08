namespace Ploeh.Katas.ArgsCSharp
{
    internal class StringParser
    {
        private char flagName;

        public StringParser(char flagName)
        {
            this.flagName = flagName;
        }

        internal Validated<string, string> TryParse(string candidate)
        {
            var idx = candidate.IndexOf($"-{flagName}");

            var nextFlagIdx = candidate[(idx + 2)..].IndexOf('-');
            var sFlag = FindValue(candidate, idx, nextFlagIdx);
            return Validated.Succeed<string, string>(sFlag);
        }

        private static string FindValue(string candidate, int idx, int nextFlagIdx)
        {
            if (nextFlagIdx < 0)
                return candidate[(idx + 2)..].Trim();

            return candidate.Substring(idx + 2, nextFlagIdx).Trim();
        }
    }
}