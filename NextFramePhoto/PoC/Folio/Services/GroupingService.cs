using Folio.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Folio.Services;

/// <inheritdoc />
public sealed class GroupingService : IGroupingService
{
    public IReadOnlyList<PhotoGroup> GroupByMonth(
        IEnumerable<Photo> photos,
        IReadOnlyList<Device> devices)
    {
        var deviceMap = devices.ToDictionary(d => d.Id);

        return photos
            .GroupBy(p => new DateTime(p.DateTaken.Year, p.DateTaken.Month, 1))
            .OrderByDescending(g => g.Key)
            .Select(g =>
            {
                var groupPhotos = g.OrderByDescending(p => p.DateTaken).ToList();
                var deviceIds   = groupPhotos.Select(p => p.DeviceId).Distinct();
                var contributors = deviceIds
                    .Where(deviceMap.ContainsKey)
                    .Select(id => deviceMap[id])
                    .ToList();

                return new PhotoGroup
                {
                    Month                = g.Key,
                    Photos               = groupPhotos,
                    ContributingDevices  = contributors,
                    EventLabel           = InferEventLabel(g.Key)
                };
            })
            .ToList();
    }

    public IReadOnlyList<TimelineEntry> BuildTimeline(IEnumerable<Photo> photos)
    {
        var months = photos
            .Select(p => new DateTime(p.DateTaken.Year, p.DateTaken.Month, 1))
            .Distinct()
            .OrderByDescending(d => d)
            .ToList();

        if (months.Count == 0)
            return [];

        var photosByMonth = photos
            .GroupBy(p => new DateTime(p.DateTaken.Year, p.DateTaken.Month, 1))
            .ToDictionary(g => g.Key, g => g.Count());

        var entries = new List<TimelineEntry>();
        int? lastYear = null;

        foreach (var month in months)
        {
            bool isYearBoundary = lastYear.HasValue && month.Year != lastYear.Value;
            entries.Add(new TimelineEntry
            {
                Date            = month,
                PhotoCount      = photosByMonth.GetValueOrDefault(month, 0),
                IsYearBoundary  = isYearBoundary
            });
            lastYear = month.Year;
        }

        return entries;
    }

    private static string? InferEventLabel(DateTime month) =>
        // In a real app this would query an event/tag table.
        (month.Year, month.Month) switch
        {
            (2024, 11) => "Prague Trip",
            (2024, 10) => "School Play",
            (2024, 9)  => "Vienna Weekend",
            _          => null
        };
}
