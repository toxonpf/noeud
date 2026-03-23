using Noeud.Domain;

namespace Noeud.Application;

public sealed class ExplorerSelectionUseCase : IExplorerSelectionUseCase
{
    public FilePath? CurrentPath { get; private set; }

    public void Select(string? path)
    {
        CurrentPath = string.IsNullOrWhiteSpace(path)
            ? null
            : FilePath.From(path);
    }
}
