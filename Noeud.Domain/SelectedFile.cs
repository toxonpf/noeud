namespace Noeud.Domain;

public sealed record SelectedFile(string Name, FilePath Path)
{
    public static SelectedFile From(string name, string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new SelectedFile(name, FilePath.From(path));
    }
}
