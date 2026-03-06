using System;

namespace Folio.Models;

/// <summary>
/// Represents a single month tick on the vertical timeline.
/// </summary>
public sealed class TimelineEntry
{
    public required DateTime Date { get; init; }
    public required int PhotoCount { get; init; }
    public bool IsYearBoundary { get; init; }

    public string MonthLabel => Date.ToString("MMM").ToUpperInvariant();
    public string YearLabel => Date.ToString("yyyy");
}
