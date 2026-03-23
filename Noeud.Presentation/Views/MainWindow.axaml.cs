using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

using System.IO;
using System.Linq;

namespace Noeud.Presentation.Views;

public partial class MainWindow : Window
{
    // Запоминаем текущую нажатую кнопку
    private Control? _activeTab;

    public MainWindow()
    {
        InitializeComponent();

        // Подписываемся на изменение размера сетки с кнопками (чтобы каретка не съезжала)
        ExplorerPagesBlock.SizeChanged += ExplorerPagesBlock_SizeChanged;
    }

    // 1. ПЕРЕКЛЮЧЕНИЕ ТИПА ПРОВОДНИКА
    private void ExplorerPageButton_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
        if (sender is Control clickedButton)
        {
            double targetX = clickedButton.Bounds.X;
            if (ExplorerPageSelectionIndicator.RenderTransform is TranslateTransform transform)
                transform.X = targetX;

            _activeTab = clickedButton;
        }
    }

    // 2. ФИКС ДЛЯ ИЗМЕНЕНИЯ РАЗМЕРА ОКНА
    private void ExplorerPagesBlock_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        // Если вкладка уже была выбрана, обновляем позицию каретки при ресайзе
        if (_activeTab != null && ExplorerPageSelectionIndicator.RenderTransform is TranslateTransform transform)
        {
            transform.X = _activeTab.Bounds.X;
        }
    }

    // 3. МЕТОД ДЛЯ ПЕРЕТАСКИВАНИЯ ОКНА ЗА ШАПКУ
    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source == sender && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            this.BeginMoveDrag(e);
        }
    }
}
