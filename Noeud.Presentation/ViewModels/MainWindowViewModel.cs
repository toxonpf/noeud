using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Noeud.Presentation.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<ExplorerItemViewModel> ExplorerItems { get; } = [];

    public MainWindowViewModel()
    {
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
