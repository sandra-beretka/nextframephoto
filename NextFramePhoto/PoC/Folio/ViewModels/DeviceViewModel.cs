using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Folio.Models;
using System;

namespace Folio.ViewModels;

/// <summary>
/// Wraps a <see cref="Device"/> for display in the filter panel.
/// </summary>
public sealed partial class DeviceViewModel : ViewModelBase
{
    private readonly Device _device;

    public DeviceViewModel(Device device) => _device = device;

    public Guid   Id          => _device.Id;
    public string Name        => _device.Name;
    public string Owner       => _device.Owner;
    public string DisplayName => $"{_device.Name} ({_device.Owner})";
    public int    PhotoCount  => _device.PhotoCount;
    public Color  AccentColor => _device.AccentColor;

    public string PhotoCountLabel => _device.PhotoCount switch
    {
        >= 1000 => $"{_device.PhotoCount / 1000.0:0.#}k",
        _       => _device.PhotoCount.ToString()
    };

    public string FormatLabel =>
        _device.Type == DeviceType.Camera ? "RAW+JPEG" :
        _device.Type == DeviceType.Phone  ? "HEIC"     : "JPEG";

    [ObservableProperty]
    private bool _isEnabled = true;

    partial void OnIsEnabledChanged(bool value) =>
        IsEnabledChanged?.Invoke(this, value);

    public event EventHandler<bool>? IsEnabledChanged;
}
