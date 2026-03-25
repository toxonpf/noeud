using CommunityToolkit.Mvvm.ComponentModel;

namespace Noeud.Presentation.Features.Canvas.ViewModels;

public class CanvasItemViewModel : ObservableObject
{
    private double _x;
    private double _y;
    private double _width;
    private double _height;
    private string _color;
    private bool _isSelected;

    public double X
    {
        get => _x;
        set => SetProperty(ref _x, value);
    }

    public double Y
    {
        get => _y;
        set => SetProperty(ref _y, value);
    }

    public double Width
    {
        get => _width;
        set => SetProperty(ref _width, value);
    }

    public double Height
    {
        get => _height;
        set => SetProperty(ref _height, value);
    }

    public string Color
    {
        get => _color;
        set => SetProperty(ref _color, value);
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }
}
