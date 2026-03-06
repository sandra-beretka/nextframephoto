using System;
using System.Collections.Generic;

namespace Folio.Models;

/// <summary>
/// A group of photos clustered by month (or event) for timeline display.
/// </summary>
public sealed class PhotoGroup
{
    public required DateTime Month { get; init; }
    public string? EventLabel { get; init; }
    public required IReadOnlyList<Photo> Photos { get; init; }
    public required IReadOnlyList<Device> ContributingDevices { get; init; }

    public string MonthLabel => Month.ToString("MMMM yyyy");
    public int PhotoCount => Photos.Count;
}
