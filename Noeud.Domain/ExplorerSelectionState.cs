namespace Noeud.Domain;

public sealed class ExplorerSelectionState
{
    public SelectedFile? CurrentFile { get; private set; }

    public void SetCurrentFile(SelectedFile? file)
    {
        CurrentFile = file;
    }
}
