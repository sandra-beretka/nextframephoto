using CommunityToolkit.Mvvm.ComponentModel;
using Folio.Models;
using System;

namespace Folio.ViewModels;

/// <summary>
/// Wraps a <see cref="TimelineEntry"/> for the vertical timeline strip.
/// </summary>
public sealed partial class TimelineEntryViewModel : ViewModelBase
{
    public TimelineEntryViewModel(TimelineEntry entry)
    {
        MonthLabel     = entry.MonthLabel;
        YearLabel      = entry.YearLabel;
        PhotoCount     = entry.PhotoCount;
        IsYearBoundary = entry.IsYearBoundary;
        Date           = entry.Date;
    }

    public string  MonthLabel     { get; }
    public string  YearLabel      { get; }
    public int     PhotoCount     { get; }
    public bool    IsYearBoundary { get; }
    public DateTime Date          { get; }

    [ObservableProperty]
    private bool _isActive;
}
