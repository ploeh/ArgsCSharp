namespace Ploeh.Katas.ArgsCSharp;

public interface IParser<T>
{
    Validated<string, T> TryParse(string candidate);
}
