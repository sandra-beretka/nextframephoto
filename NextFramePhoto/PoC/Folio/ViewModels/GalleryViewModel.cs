using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Folio.Models;
using Folio.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.ViewModels;

/// <summary>
/// State for Column 2: grouped photo grid and vertical timeline scrubber.
/// </summary>
public sealed partial class GalleryViewModel : ViewModelBase
{
    private readonly IPhotoRepository _repository;
    private readonly IGroupingService  _groupingService;

    public GalleryViewModel(IPhotoRepository repository, IGroupingService groupingService)
    {
        _repository      = repository;
        _groupingService = groupingService;
    }

    public ObservableCollection<PhotoGroupViewModel>   Groups         { get; } = [];
    public ObservableCollection<TimelineEntryViewModel> TimelineEntries { get; } = [];

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _activeCameraFilter = "All";

    /// <summary>Raised when the user clicks a photo thumbnail.</summary>
    public event EventHandler<(PhotoViewModel Photo, PhotoGroupViewModel Group)>? PhotoSelected;

    public async Task LoadAsync(System.Collections.Generic.IReadOnlyList<DeviceViewModel> devices)
    {
        IsLoading = true;
        try
        {
            var allPhotos = await _repository.GetAllPhotosAsync();
            var allDevices = await _repository.GetAllDevicesAsync();
            var deviceMap  = devices.ToDictionary(d => d.Id);

            var groups = _groupingService.GroupByMonth(allPhotos, allDevices);
            Groups.Clear();

            // Inbox group (uncataloged) always first
            var uncataloged = allPhotos.Where(p => !p.IsCataloged).ToList();
            if (uncataloged.Count > 0)
            {
                var inboxPhotos = uncataloged
                    .Where(p => deviceMap.ContainsKey(p.DeviceId))
                    .Select(p => MakePhotoVM(p, deviceMap))
                    .ToList();

                var inboxDevices = uncataloged
                    .Select(p => p.DeviceId).Distinct()
                    .Where(deviceMap.ContainsKey)
                    .Select(id => deviceMap[id])
                    .ToList();

                Groups.Add(new PhotoGroupViewModel(
                    new PhotoGroup
                    {
                        Month               = DateTime.Today,
                        EventLabel          = "INBOX — Uncataloged",
                        Photos              = uncataloged,
                        ContributingDevices = inboxDevices.Select(d =>
                            allDevices.First(ad => ad.Id == d.Id)).ToList()
                    },
                    new ObservableCollection<PhotoViewModel>(inboxPhotos)));
            }

            foreach (var group in groups)
            {
                var photoVMs = group.Photos
                    .Where(p => p.IsCataloged && deviceMap.ContainsKey(p.DeviceId))
                    .Select(p => MakePhotoVM(p, deviceMap))
                    .ToList();

                if (photoVMs.Count == 0) continue;

                var groupVm = new PhotoGroupViewModel(group,
                    new ObservableCollection<PhotoViewModel>(photoVMs));

                foreach (var photo in groupVm.Photos)
                    photo.PropertyChanged += (_, _) => { };

                Groups.Add(groupVm);
            }

            // Build timeline
            var timeline = _groupingService.BuildTimeline(allPhotos);
            TimelineEntries.Clear();
            foreach (var entry in timeline)
                TimelineEntries.Add(new TimelineEntryViewModel(entry));

            if (TimelineEntries.Count > 0)
                TimelineEntries[0].IsActive = true;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void SelectPhoto(PhotoViewModel photo)
    {
        // Deselect all others
        foreach (var g in Groups)
            foreach (var p in g.Photos)
                p.IsSelected = false;

        photo.IsSelected = true;

        var group = Groups.FirstOrDefault(g => g.Photos.Contains(photo));
        if (group is not null)
            PhotoSelected?.Invoke(this, (photo, group));
    }

    [RelayCommand]
    private void ActivateTimelineEntry(TimelineEntryViewModel entry)
    {
        foreach (var e in TimelineEntries)
            e.IsActive = false;
        entry.IsActive = true;
        // In a real app: scroll gallery to the corresponding month group
    }

    private static PhotoViewModel MakePhotoVM(
        Photo photo,
        System.Collections.Generic.Dictionary<Guid, DeviceViewModel> deviceMap)
    {
        var deviceVm = deviceMap[photo.DeviceId];
        return new PhotoViewModel(photo, deviceVm)
        {
            IsStarred = photo.IsStarred,
            Status    = photo.Status
        };
    }
}
