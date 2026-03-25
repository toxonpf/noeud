using Noeud.Application;
using Noeud.Presentation.Features.Editor.ViewModels;
using Noeud.Presentation.Features.Explorer.ViewModels;
using Noeud.Presentation.Shared.ViewModels;

namespace Noeud.Presentation.Shell;

public partial class MainWindowViewModel : ViewModelBase
{
    public ExplorerPanelViewModel Explorer { get; }
    public EditorPanelViewModel Editor { get; }

    public MainWindowViewModel() : this(new ExplorerSelectionUseCase())
    {
    }

    public MainWindowViewModel(IExplorerSelectionUseCase explorerSelectionUseCase)
    {
        Explorer = new ExplorerPanelViewModel(explorerSelectionUseCase);
        Editor = new EditorPanelViewModel();
    }
}
