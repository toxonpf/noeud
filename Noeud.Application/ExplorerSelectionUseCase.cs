using Noeud.Domain;

namespace Noeud.Application;

public sealed class ExplorerSelectionUseCase : IExplorerSelectionUseCase
{
    private readonly ExplorerSelectionState _selectionState;

    public ExplorerSelectionUseCase() : this(new ExplorerSelectionState())
    {
    }

    public ExplorerSelectionUseCase(ExplorerSelectionState selectionState)
    {
        _selectionState = selectionState;
    }

    public FilePath? CurrentPath => CurrentFile?.Path;

    public SelectedFile? CurrentFile => _selectionState.CurrentFile;

    public void SelectFile(string? name, string? path)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(path))
        {
            _selectionState.SetCurrentFile(null);
            return;
        }

        _selectionState.SetCurrentFile(SelectedFile.From(name, path));
    }

    public void ClearSelection()
    {
        _selectionState.SetCurrentFile(null);
    }
}
