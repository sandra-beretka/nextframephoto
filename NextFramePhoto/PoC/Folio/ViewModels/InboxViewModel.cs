using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Folio.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.ViewModels;

/// <summary>
/// Encapsulates inbox / uncataloged-batch state shown at the bottom of column 1.
/// </summary>
public sealed partial class InboxViewModel : ViewModelBase
{
    private readonly IPhotoRepository _repository;

    public InboxViewModel(IPhotoRepository repository)
    {
        _repository = repository;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ProgressPercent))]
    [NotifyPropertyChangedFor(nameof(DoneLabel))]
    private int _uncatalogedCount;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ProgressPercent))]
    [NotifyPropertyChangedFor(nameof(DoneLabel))]
    private int _totalCount;

    [ObservableProperty]
    private string _lastImportLabel = "No imports yet";

    public IReadOnlyList<InboxSourceViewModel> Sources { get; private set; } = [];

    public double ProgressPercent =>
        TotalCount > 0 ? (double)(TotalCount - UncatalogedCount) / TotalCount : 0;

    public string DoneLabel =>
        $"{TotalCount - UncatalogedCount} done · {ProgressPercent:P0} complete";

    public async Task LoadAsync(IReadOnlyList<DeviceViewModel> devices)
    {
        var uncataloged = await _repository.GetUncatalogedPhotosAsync();
        var all         = await _repository.GetAllPhotosAsync();

        UncatalogedCount = uncataloged.Count;
        TotalCount       = UncatalogedCount + (all.Count - uncataloged.Count);

        var deviceMap = devices.ToDictionary(d => d.Id);

        Sources = uncataloged
            .GroupBy(p => p.DeviceId)
            .Where(g => deviceMap.ContainsKey(g.Key))
            .Select(g => new InboxSourceViewModel(deviceMap[g.Key], g.Count()))
            .ToList();

        if (uncataloged.Count > 0)
        {
            var latest = uncataloged.Max(p => p.DateTaken);
            var delta  = DateTime.Now - latest;
            LastImportLabel = delta.TotalDays switch
            {
                < 1  => "Imported today",
                < 2  => "Imported yesterday",
                <= 7 => $"{(int)delta.TotalDays} days ago",
                _    => $"Last: {latest:MMM d}"
            };
        }

        OnPropertyChanged(nameof(Sources));
    }

    /// <summary>Raised when the user clicks "Catalog now".</summary>
    public event EventHandler? CatalogRequested;

    [RelayCommand]
    private void RequestCatalog() => CatalogRequested?.Invoke(this, EventArgs.Empty);
}

/// <summary>Per-device summary row inside the inbox card.</summary>
public sealed class InboxSourceViewModel(DeviceViewModel device, int count)
{
    public DeviceViewModel Device { get; } = device;
    public int             Count  { get; } = count;
}
