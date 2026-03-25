using System.Collections.ObjectModel;

using Noeud.Presentation.Shared.ViewModels;

namespace Noeud.Presentation.Features.Explorer.ViewModels;

public class ExplorerItemViewModel : ViewModelBase
{
    private bool _isExpanded;

    public string Name { get; }
    public string FullPath { get; }
    public bool IsDirectory { get; }

    public ObservableCollection<ExplorerItemViewModel> Children { get; } = [];

    public bool IsExpanded
    {
        get => _isExpanded;
        set => SetProperty(ref _isExpanded, value);
    }

    public ExplorerItemViewModel(string name, string fullPath, bool isDirectory)
    {
        Name = name;
        FullPath = fullPath;
        IsDirectory = isDirectory;
    }
}
