using Noeud.Domain;

namespace Noeud.Application;

public interface IExplorerSelectionUseCase
{
    FilePath? CurrentPath { get; }

    void Select(string? path);
}
