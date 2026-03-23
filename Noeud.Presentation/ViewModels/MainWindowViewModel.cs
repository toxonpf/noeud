using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Noeud.Application;

namespace Noeud.Presentation.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IExplorerSelectionUseCase _explorerSelectionUseCase;
    private ExplorerItemViewModel? _selectedExplorerItem;
    private string? _selectedPath;

    public ObservableCollection<ExplorerItemViewModel> ExplorerItems { get; } = [];

    public ExplorerItemViewModel? SelectedExplorerItem
    {
        get => _selectedExplorerItem;
        set
        {
            if (!SetProperty(ref _selectedExplorerItem, value))
                return;

            SelectedPath = value?.FullPath;

            if (value is null || value.IsDirectory)
            {
                _explorerSelectionUseCase.ClearSelection();
                return;
            }

            _explorerSelectionUseCase.SelectFile(value.Name, value.FullPath);
        }
    }

    public string? SelectedPath
    {
        get => _selectedPath;
        private set => SetProperty(ref _selectedPath, value);
    }

    public MainWindowViewModel() : this(new ExplorerSelectionUseCase())
    {
    }

    public MainWindowViewModel(IExplorerSelectionUseCase explorerSelectionUseCase)
    {
        _explorerSelectionUseCase = explorerSelectionUseCase;
        GenerateExplorerContent("/home/toxonpf/Disk/prl/codes/cs/projects/Noeud/Noeud.Presentation/Assets/explorer");
    }

    private void GenerateExplorerContent(string rootPath)
    {
        ExplorerItems.Clear();

        var root = new DirectoryInfo(rootPath);
        if (!root.Exists) return;

        foreach (var directory in root.GetDirectories().OrderBy(x => x.Name))
            ExplorerItems.Add(BuildDirectoryNode(directory));

        foreach (var file in root.GetFiles().OrderBy(x => x.Name))
            ExplorerItems.Add(new ExplorerItemViewModel(file.Name, file.FullName, false));
    }

    private ExplorerItemViewModel BuildDirectoryNode(DirectoryInfo directory)
    {
        var node = new ExplorerItemViewModel(directory.Name, directory.FullName, true);

        foreach (var childDirectory in directory.GetDirectories().OrderBy(x => x.Name))
            node.Children.Add(BuildDirectoryNode(childDirectory));

        foreach (var childFile in directory.GetFiles().OrderBy(x => x.Name))
            node.Children.Add(new ExplorerItemViewModel(childFile.Name, childFile.FullName, false));

        return node;
    }
}
