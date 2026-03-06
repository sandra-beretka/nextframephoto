using Folio.Models;
using System.Collections.Generic;

namespace Folio.Services;

/// <summary>
/// Builds grouped timeline data from a flat photo collection.
/// </summary>
public interface IGroupingService
{
    IReadOnlyList<PhotoGroup> GroupByMonth(IEnumerable<Photo> photos, IReadOnlyList<Device> devices);
    IReadOnlyList<TimelineEntry> BuildTimeline(IEnumerable<Photo> photos);
}
