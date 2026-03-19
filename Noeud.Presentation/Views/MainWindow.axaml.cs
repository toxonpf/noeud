using Avalonia.Controls;
using Avalonia.Input;

namespace Noeud.Presentation.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // Проверяем, что нажата именно левая кнопка мыши
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            // Запускаем системный процесс перетаскивания окна
            this.BeginMoveDrag(e);
        }
    }
}
