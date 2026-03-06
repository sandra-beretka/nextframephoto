using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Folio.ViewModels;
using System;

namespace Folio.Views.Controls;

/// <summary>
/// Renders a single photo thumbnail. Generates a deterministic placeholder
/// gradient from the photo's filename hash until real image loading is wired.
/// </summary>
public partial class PhotoThumbnailControl : UserControl
{
    // Gradient palette for placeholder thumbnails
    private static readonly (Color from, Color to)[] _palettes =
    [
        (Color.Parse("#1A2530"), Color.Parse("#0D1820")),
        (Color.Parse("#241820"), Color.Parse("#180D15")),
        (Color.Parse("#1A2418"), Color.Parse("#0D1810")),
        (Color.Parse("#241E18"), Color.Parse("#18140D")),
        (Color.Parse("#18182E"), Color.Parse("#0D0D20")),
        (Color.Parse("#241818"), Color.Parse("#180D0D")),
        (Color.Parse("#1E2418"), Color.Parse("#141810")),
        (Color.Parse("#1A2030"), Color.Parse("#101520")),
        (Color.Parse("#281818"), Color.Parse("#1A0D0D")),
        (Color.Parse("#181828"), Color.Parse("#0D0D1A")),
        (Color.Parse("#1E2818"), Color.Parse("#12180D")),
        (Color.Parse("#28201A"), Color.Parse("#1A140D")),
    ];

    public PhotoThumbnailControl()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
        PointerEntered     += OnPointerEntered;
        PointerExited      += OnPointerExited;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is PhotoViewModel vm)
            ApplyPlaceholderGradient(vm.FileName);
    }

    private void ApplyPlaceholderGradient(string fileName)
    {
        var bg = this.FindControl<Border>("PlaceholderBg");
        if (bg is null) return;

        int idx = Math.Abs(fileName.GetHashCode()) % _palettes.Length;
        var (from, to) = _palettes[idx];

        bg.Background = new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint   = new RelativePoint(1, 1, RelativeUnit.Relative),
            GradientStops =
            [
                new GradientStop(from, 0),
                new GradientStop(to,   1)
            ]
        };
    }

    private void OnPointerEntered(object? sender, PointerEventArgs e)
    {
        var overlay = this.FindControl<Border>("HoverOverlay");
        if (overlay is not null) overlay.IsVisible = true;
    }

    private void OnPointerExited(object? sender, PointerEventArgs e)
    {
        var overlay = this.FindControl<Border>("HoverOverlay");
        if (overlay is not null) overlay.IsVisible = false;
    }
}
