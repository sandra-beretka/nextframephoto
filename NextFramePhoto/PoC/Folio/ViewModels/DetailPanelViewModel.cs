using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Folio.Models;
using Folio.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.ViewModels;

/// <summary>
/// State for Column 3: photo preview, EXIF metadata, event cluster, actions.
/// </summary>
public sealed partial class DetailPanelViewModel : ViewModelBase
{
    private readonly IPhotoRepository _repository;

    public DetailPanelViewModel(IPhotoRepository repository)
    {
        _repository = repository;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasSelection))]
    [NotifyPropertyChangedFor(nameof(EventCluster))]
    private PhotoViewModel? _selectedPhoto;

    public bool HasSelection => SelectedPhoto is not null;

    [ObservableProperty]
    private EventClusterViewModel? _eventCluster;

    partial void OnSelectedPhotoChanged(PhotoViewModel? value)
    {
        if (value is null)
            EventCluster = null;
    }

    public async Task SelectPhotoAsync(PhotoViewModel photo, System.Collections.Generic.IReadOnlyList<PhotoViewModel> siblings)
    {
        SelectedPhoto = photo;

        // Build event cluster from sibling photos in the same group
        var siblingDevices = siblings
            .GroupBy(p => p.Device.Id)
            .Select(g => new EventClusterDeviceViewModel(g.First().Device, g.Count()))
            .ToList();

        EventCluster = new EventClusterViewModel(
            label: photo.Device.Name.Contains("Sony") ? "Same Event Detected" : "Multi-device event",
            deviceCount: siblingDevices.Count,
            devices: siblingDevices);

        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task CatalogSelectedAsync()
    {
        if (SelectedPhoto is null) return;
        await _repository.MarkCatalogedAsync(SelectedPhoto.Id);
        SelectedPhoto.Status = CatalogStatus.Done;
        PhotoCataloged?.Invoke(this, SelectedPhoto);
    }

    [RelayCommand]
    private async Task ToggleStarAsync()
    {
        if (SelectedPhoto is null) return;
        SelectedPhoto.IsStarred = !SelectedPhoto.IsStarred;
        await _repository.SetStarredAsync(SelectedPhoto.Id, SelectedPhoto.IsStarred);
    }

    public event EventHandler<PhotoViewModel>? PhotoCataloged;
}

public sealed class EventClusterViewModel(
    string label,
    int deviceCount,
    System.Collections.Generic.IReadOnlyList<EventClusterDeviceViewModel> devices)
{
    public string  Label       { get; } = label;
    public string  SubLabel    => $"{deviceCount} devices · overlapping window";
    public System.Collections.Generic.IReadOnlyList<EventClusterDeviceViewModel> Devices { get; } = devices;
}

public sealed class EventClusterDeviceViewModel(DeviceViewModel device, int count)
{
    public DeviceViewModel Device { get; } = device;
    public int             Count  { get; } = count;
    public string          Label  => $"{Device.Name} · {Count}";
}
