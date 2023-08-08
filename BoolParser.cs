namespace Ploeh.Katas.ArgsCSharp
{
    internal class BoolParser
    {
        private string v;

        public BoolParser(string v)
        {
            this.v = v;
        }

        internal Validated<string, bool> Parse(string v)
        {
            return Validated<string, bool>.Succeed(true);
        }
    }
}