using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Folio.Models;

namespace Folio.ViewModels;

/// <summary>
/// Wraps a <see cref="PhotoGroup"/> for display as a labeled section in the gallery.
/// </summary>
public sealed class PhotoGroupViewModel : ViewModelBase
{
    public PhotoGroupViewModel(PhotoGroup group, ObservableCollection<PhotoViewModel> photos)
    {
        MonthLabel           = group.MonthLabel;
        EventLabel           = group.EventLabel;
        ContributingDevices  = [.. group.ContributingDevices.Select(d => new DeviceViewModel(d))];
        Photos               = photos;
    }

    public string  MonthLabel          { get; }
    public string? EventLabel          { get; }

    public string HeaderLabel =>
        EventLabel is { Length: > 0 } ev ? $"{MonthLabel} · {ev}" : MonthLabel;

    public ObservableCollection<PhotoViewModel>  Photos               { get; }
    public IReadOnlyList<DeviceViewModel>         ContributingDevices  { get; }
    public int PhotoCount => Photos.Count;
}
