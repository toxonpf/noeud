namespace Noeud.Domain;

public sealed record FilePath(string Value)
{
    public static FilePath From(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        return new FilePath(path);
    }

    public override string ToString() => Value;
}
