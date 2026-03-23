using Noeud.Domain;

namespace Noeud.Application;

public interface IExplorerSelectionUseCase
{
    FilePath? CurrentPath { get; }
    SelectedFile? CurrentFile { get; }

    void SelectFile(string? name, string? path);

    void ClearSelection();
}
