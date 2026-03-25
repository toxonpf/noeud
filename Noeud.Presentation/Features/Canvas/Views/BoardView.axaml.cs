using System;
using System.Collections.ObjectModel;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

using Noeud.Presentation.Features.Canvas.ViewModels;

namespace Noeud.Presentation.Features.Canvas.Views;

public partial class BoardView : UserControl
{
    private bool _isPanning;
    private Point _startMousePosition;
    private Point _startPanOffset;

    public BoardView()
    {
        InitializeComponent();
        var initialItems = new ObservableCollection<CanvasItemViewModel>
        {
            new CanvasItemViewModel { X = 100, Y = 100, Width = 50, Height = 100, Color = "red"},
            new CanvasItemViewModel { X = 400, Y = 100, Width = 50, Height = 100, Color = "blue"}
        };
        DataContext = new BoardViewModel(initialItems);
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs eventArgs)
    {
        if (eventArgs.GetCurrentPoint(this).Properties.IsMiddleButtonPressed)
        {
            if (DataContext is BoardViewModel viewModel && sender is Control senderControl)
            {
                _startMousePosition = eventArgs.GetPosition(this);
                _startPanOffset = new Point(viewModel.PanX, viewModel.PanY);
                eventArgs.Pointer.Capture(senderControl);
                _isPanning = true;
            }
        }
    }

    private void InputElement_OnPointerMoved(object? sender, PointerEventArgs eventArgs)
    {
        if (!_isPanning) return;

        if (DataContext is BoardViewModel viewModel)
        {
            var currentPosition = eventArgs.GetPosition(this);

            var deltaX = currentPosition.X - _startMousePosition.X;
            var deltaY = currentPosition.Y - _startMousePosition.Y;

            viewModel.PanX = _startPanOffset.X + deltaX;
            viewModel.PanY = _startPanOffset.Y + deltaY;
        }
    }

    private void InputElement_OnPointerReleased(object? sender, PointerReleasedEventArgs eventArgs)
    {
        _isPanning = false;
        eventArgs.Pointer.Capture(null);
    }

    private void InputElement_OnPointerWheelChanged(object? sender, PointerWheelEventArgs eventArgs)
    {
        if (DataContext is BoardViewModel viewModel)
        {
            var mousePas = eventArgs.GetPosition(this);
            const double zoomFactor = 1.3;

            double newZoom = viewModel.Zoom;

            if (eventArgs.Delta.Y > 0) // Крутим вверх (Приближение)
            {
                newZoom = Math.Clamp(viewModel.Zoom * zoomFactor, 0.1, 10.0);
            }
            else if (eventArgs.Delta.Y < 0) // Крутим вниз (Отдаление)
            {
                newZoom = Math.Clamp(viewModel.Zoom / zoomFactor, 0.1, 10.0);
            }

            if (newZoom == viewModel.Zoom) return;

            viewModel.PanX = mousePas.X - (mousePas.X - viewModel.PanX) * (newZoom / viewModel.Zoom);
            viewModel.PanY = mousePas.Y - (mousePas.Y - viewModel.PanY) * (newZoom / viewModel.Zoom);

            viewModel.Zoom = newZoom;
        }
    }
}
