using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Noeud.Presentation.Features.Canvas.ViewModels;

public class BoardViewModel : ObservableObject
{
    private double _zoom = 1.0;
    private double _panX = 0.0;
    private double _panY = 0.0;

    public ObservableCollection<CanvasItemViewModel> Items { get; }

    public BoardViewModel(ObservableCollection<CanvasItemViewModel> items)
    {
        Items = items;
    }

    public double Zoom
    {
        get => _zoom;
        set => SetProperty(ref _zoom, value);
    }

    public double PanX
    {
        get => _panX;
        set => SetProperty(ref _panX, value);
    }

    public double PanY
    {
        get => _panY;
        set => SetProperty(ref _panY, value);
    }
}
