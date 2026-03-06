using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Folio.Models;
using System;

namespace Folio.ViewModels;

/// <summary>
/// Wraps a <see cref="Photo"/> for display in the gallery thumbnail grid.
/// </summary>
public sealed partial class PhotoViewModel : ViewModelBase
{
    private readonly Photo _photo;

    public PhotoViewModel(Photo photo, DeviceViewModel device)
    {
        _photo = photo;
        Device = device;
    }

    public Guid     Id           => _photo.Id;
    public string   FileName     => _photo.FileName;
    public DateTime DateTaken    => _photo.DateTaken;
    public string?  GpsLocation  => _photo.GpsLocation;
    public string?  CameraModel  => _photo.CameraModel;
    public string?  LensModel    => _photo.LensModel;
    public int?     Iso          => _photo.Iso;
    public double?  Aperture     => _photo.Aperture;
    public double?  ShutterSpeed => _photo.ShutterSpeed;
    public int?     WidthPx      => _photo.WidthPx;
    public int?     HeightPx     => _photo.HeightPx;

    public DeviceViewModel Device { get; }

    [ObservableProperty]
    private bool _isStarred;

    [ObservableProperty]
    private bool _isSelected;

    [ObservableProperty]
    private CatalogStatus _status;

    public bool IsCataloged => _status == CatalogStatus.Done;

    public string ExposureLabel =>
        (ShutterSpeed, Aperture, Iso) is ({ } s, { } a, { } i)
            ? $"1/{(int)(1 / s)}s · f/{a} · ISO {i}"
            : string.Empty;

    public string DimensionsLabel =>
        (WidthPx, HeightPx) is ({ } w, { } h)
            ? $"{w} × {h}"
            : string.Empty;

    public string DateLabel => DateTaken.ToString("MMM d, yyyy · HH:mm");
}
