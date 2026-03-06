using System;

namespace Folio.Models;

/// <summary>
/// Represents a single photo in the catalog.
/// </summary>
public sealed class Photo
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string FilePath { get; init; }
    public required string FileName { get; init; }
    public required DateTime DateTaken { get; init; }
    public required Guid DeviceId { get; init; }

    // EXIF
    public string? CameraModel { get; init; }
    public string? LensModel { get; init; }
    public double? ShutterSpeed { get; init; }
    public double? Aperture { get; init; }
    public int? Iso { get; init; }
    public int? WidthPx { get; init; }
    public int? HeightPx { get; init; }
    public string? GpsLocation { get; init; }

    public bool IsCataloged { get; set; }
    public bool IsStarred { get; set; }
    public CatalogStatus Status { get; set; } = CatalogStatus.Uncataloged;
}

public enum CatalogStatus
{
    Uncataloged,
    InProgress,
    Done
}
