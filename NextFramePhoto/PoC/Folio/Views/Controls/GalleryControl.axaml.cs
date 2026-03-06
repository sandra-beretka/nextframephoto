using Avalonia.Controls;
using Avalonia.Input;
using Folio.ViewModels;

namespace Folio.Views.Controls;

public partial class GalleryControl : UserControl
{
    public GalleryControl()
    {
        InitializeComponent();
        // Thumbnail click bubbles up via PointerPressed; handled in MainWindow
        AddHandler(PointerPressedEvent, OnAnyPointerPressed, handledEventsToo: false);
    }

    private void OnAnyPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is not GalleryViewModel vm) return;

        // Walk up visual tree to find a PhotoThumbnailControl
        var src = e.Source as Avalonia.Visual;
        while (src is not null)
        {
            if (src is PhotoThumbnailControl thumb &&
                thumb.DataContext is PhotoViewModel photo)
            {
                vm.SelectPhotoCommand.Execute(photo);
                return;
            }
            src = src.Parent as Avalonia.Visual; //.VisualParent;
        }
    }
}
