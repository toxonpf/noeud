using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace Noeud.Presentation.Shell;

public partial class MainWindow : Window
{
    private Control? _activeTab;

    public MainWindow()
    {
        InitializeComponent();

        _activeTab = ExplorerPageButtonHide;
        ExplorerPagesBlock.SizeChanged += ExplorerPagesBlock_SizeChanged;
    }

    private void ExplorerPageButton_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            return;

        if (sender is not Control clickedButton)
            return;

        if (ExplorerPageSelectionIndicator.RenderTransform is TranslateTransform transform)
        {
            transform.X = clickedButton.Bounds.X;
        }

        _activeTab = clickedButton;
    }

    private void ExplorerPagesBlock_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (_activeTab is null || ExplorerPageSelectionIndicator.RenderTransform is not TranslateTransform transform)
            return;

        transform.X = _activeTab.Bounds.X;
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source == sender && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }
}
