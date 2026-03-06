using Avalonia.Controls;
using Avalonia.Input;

namespace Folio.Views.Controls;

public partial class DeviceRowControl : UserControl
{
    public DeviceRowControl() => InitializeComponent();

    private void OnPointerEntered(object? sender, PointerEventArgs e)
    {
        if (sender is Border b)
            b.Background = App.Current?.FindResource("Brush.Surface2") as Avalonia.Media.IBrush;
    }

    private void OnPointerExited(object? sender, PointerEventArgs e)
    {
        if (sender is Border b)
            b.Background = Avalonia.Media.Brushes.Transparent;
    }
}
