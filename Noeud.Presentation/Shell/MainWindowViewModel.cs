using Noeud.Application;
using Noeud.Presentation.Features.Editor.ViewModels;
using Noeud.Presentation.Features.Explorer.ViewModels;
using Noeud.Presentation.Shared.ViewModels;

namespace Noeud.Presentation.Shell;

public class MainWindowViewModel(IExplorerSelectionUseCase explorerSelectionUseCase) : ViewModelBase
{
    public ExplorerPanelViewModel Explorer { get; } = new(explorerSelectionUseCase);
    public EditorPanelViewModel Editor { get; } = new();

    public MainWindowViewModel() : this(new ExplorerSelectionUseCase())
    {
    }
}
