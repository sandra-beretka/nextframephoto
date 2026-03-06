using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Folio.Services;
using System.Threading.Tasks;

namespace Folio.ViewModels;

/// <summary>
/// Root view model for the main window.
/// Owns and coordinates the three column view models.
/// </summary>
public sealed partial class MainWindowViewModel : ViewModelBase
{
    private readonly IPhotoRepository _repository;
    private readonly IGroupingService  _groupingService;

    public MainWindowViewModel()
        : this(new DesignTimePhotoRepository(), new GroupingService()) { }

    public MainWindowViewModel(IPhotoRepository repository, IGroupingService groupingService)
    {
        _repository      = repository;
        _groupingService = groupingService;

        var inbox = new InboxViewModel(_repository);
        FilterPanel  = new FilterPanelViewModel(_repository, inbox);
        Gallery      = new GalleryViewModel(_repository, _groupingService);
        DetailPanel  = new DetailPanelViewModel(_repository);

        WireEvents();
    }

    public FilterPanelViewModel FilterPanel { get; }
    public GalleryViewModel     Gallery     { get; }
    public DetailPanelViewModel DetailPanel { get; }

    [ObservableProperty]
    private bool _isLoading;

    private void WireEvents()
    {
        // Photo selection in gallery → populate detail panel
        Gallery.PhotoSelected += async (_, args) =>
        {
            var allPhotosInGroup = args.Group.Photos;
            await DetailPanel.SelectPhotoAsync(args.Photo, allPhotosInGroup);
        };

        // Device filter toggle → reload gallery
        FilterPanel.FilterChanged += async (_, _) =>
            await Gallery.LoadAsync(FilterPanel.Devices);

        // Catalog action → refresh inbox count
        DetailPanel.PhotoCataloged += async (_, _) =>
            await FilterPanel.Inbox.LoadAsync(FilterPanel.Devices);
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsLoading = true;
        await FilterPanel.LoadAsync();
        await Gallery.LoadAsync(FilterPanel.Devices);
        IsLoading = false;
    }
}
