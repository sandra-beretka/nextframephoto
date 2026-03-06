using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Folio.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.ViewModels;

/// <summary>
/// State for Column 1: device list, mini calendar and tag filters.
/// </summary>
public sealed partial class FilterPanelViewModel : ViewModelBase
{
    private readonly IPhotoRepository _repository;

    public FilterPanelViewModel(IPhotoRepository repository, InboxViewModel inbox)
    {
        _repository = repository;
        Inbox       = inbox;
    }

    public InboxViewModel Inbox { get; }

    public ObservableCollection<DeviceViewModel> Devices { get; } = [];

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private DateTime _calendarMonth = new(DateTime.Today.Year, DateTime.Today.Month, 1);

    // Tags (static for now; in a real app loaded from the repository)
    public ObservableCollection<TagViewModel> Tags { get; } =
    [
        new TagViewModel("Prague 2024",  "#7ec8a0", "#3d6b4a", "#1a2d1f"),
        new TagViewModel("Family",       "#80b0ff", "#2a4a7f", "#1a2040"),
        new TagViewModel("Vacation",     "#ffcc80", "#6b4a1a", "#2d2010"),
        new TagViewModel("Kids",         "#d08080", "#5a2a2a", "#2a1a1a"),
        new TagViewModel("Starred ★",   "#c080d0", "#5a2a6a", "#261a2a"),
    ];

    /// <summary>Raised when the active device filter selection changes.</summary>
    public event EventHandler? FilterChanged;

    public async Task LoadAsync()
    {
        var devices = await _repository.GetAllDevicesAsync();
        Devices.Clear();
        foreach (var d in devices)
        {
            var vm = new DeviceViewModel(d);
            vm.IsEnabledChanged += (_, _) => FilterChanged?.Invoke(this, EventArgs.Empty);
            Devices.Add(vm);
        }

        await Inbox.LoadAsync(Devices);
    }

    [RelayCommand]
    private void NavigateCalendarBack()
    {
        CalendarMonth = CalendarMonth.AddMonths(-1);
    }

    [RelayCommand]
    private void NavigateCalendarForward()
    {
        CalendarMonth = CalendarMonth.AddMonths(1);
    }
}

/// <summary>A single tag chip in the filter panel.</summary>
public sealed partial class TagViewModel(
    string label,
    string textColor,
    string borderColor,
    string bgColor) : ViewModelBase
{
    public string Label       { get; } = label;
    public string TextColor   { get; } = textColor;
    public string BorderColor { get; } = borderColor;
    public string BgColor     { get; } = bgColor;

    [ObservableProperty]
    private bool _isActive;
}
