using Avalonia.Media;
using System;

namespace Folio.Models;

/// <summary>
/// Represents a camera or device that produces photos.
/// </summary>
public sealed class Device
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; init; }
    public required string Owner { get; init; }
    public DeviceType Type { get; init; }
    public required Color AccentColor { get; init; }
    public int PhotoCount { get; set; }
    public bool IsEnabled { get; set; } = true;
}

public enum DeviceType
{
    Camera,
    Phone,
    Tablet,
    ActionCamera
}
