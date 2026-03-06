# Folio — Photo Catalog App (Avalonia 11)

A prosumer photo cataloging application built with Avalonia 11, MVVM (CommunityToolkit.Mvvm), and clean architecture principles.

## Project Structure

```
Folio/
├── Models/                         # Pure domain models (no UI dependencies)
│   ├── Photo.cs                    # Single photo with EXIF data + catalog status
│   ├── Device.cs                   # Camera / phone / tablet source device
│   ├── PhotoGroup.cs               # Month-level group of photos for timeline
│   └── TimelineEntry.cs            # One tick on the vertical timeline scrubber
│
├── Services/                       # Business logic interfaces + implementations
│   ├── IPhotoRepository.cs         # Data access abstraction
│   ├── IGroupingService.cs         # Timeline grouping abstraction
│   ├── DesignTimePhotoRepository.cs # In-memory seed data (design + tests)
│   └── GroupingService.cs          # Groups photos by month, builds timeline
│
├── ViewModels/                     # MVVM view models (CommunityToolkit.Mvvm)
│   ├── ViewModelBase.cs            # Base: ObservableObject
│   ├── MainWindowViewModel.cs      # Root VM — owns and wires all sub-VMs
│   ├── FilterPanelViewModel.cs     # Col 1: devices, calendar, tags, search
│   ├── InboxViewModel.cs           # Inbox card at bottom of Col 1
│   ├── GalleryViewModel.cs         # Col 2: photo groups + timeline entries
│   ├── DetailPanelViewModel.cs     # Col 3: selected photo detail + actions
│   ├── DeviceViewModel.cs          # Wraps Device for UI
│   ├── PhotoViewModel.cs           # Wraps Photo for UI thumbnail + detail
│   ├── PhotoGroupViewModel.cs      # Wraps PhotoGroup for section header
│   └── TimelineEntryViewModel.cs   # Wraps TimelineEntry for VTL tick
│
├── Views/
│   ├── MainWindow.axaml            # Shell: top nav + 3-column grid
│   └── Controls/
│       ├── FilterPanelControl      # Column 1: search, devices, calendar, tags
│       ├── InboxPanelControl       # Inbox card (pinned bottom of Col 1)
│       ├── DeviceRowControl        # Single device row with toggle switch
│       ├── MiniCalendarControl     # Mini month calendar with photo-day dots
│       ├── GalleryControl          # Column 2: gallery grid + VTL strip
│       ├── PhotoGroupSectionControl # One month section (header + grid)
│       ├── PhotoThumbnailControl   # Single photo thumbnail with hover
│       ├── VerticalTimelineControl # Narrow right-side timeline scrubber
│       └── DetailPanelControl      # Column 3: preview, EXIF, actions
│
├── Converters/
│   └── CalendarDayConverter.cs     # DateTime → calendar label
│
└── Assets/Styles/
    ├── FolioTheme.axaml            # Color palette, brushes, typography vars
    └── Controls.axaml              # Button/toggle/chip style templates
```

## Dependencies

| Package | Version | Purpose |
|---|---|---|
| Avalonia | 11.2.3 | UI framework |
| Avalonia.Desktop | 11.2.3 | Desktop lifetime |
| Avalonia.Themes.Fluent | 11.2.3 | Base theme |
| Avalonia.Fonts.Inter | 11.2.3 | Inter font |
| CommunityToolkit.Mvvm | 8.3.2 | Source-gen MVVM (ObservableProperty, RelayCommand) |

## Setup

```bash
# 1. Restore & run
cd Folio
dotnet restore
dotnet run

# 2. Run with hot reload
dotnet watch run
```

## Architecture

### SOLID Principles Applied

- **S** — Each class has one responsibility. `GroupingService` only groups; `InboxViewModel` only tracks inbox state.
- **O** — `IPhotoRepository` and `IGroupingService` allow swapping implementations without changing consumers.
- **L** — ViewModels depend on abstractions (interfaces), not concrete types.
- **I** — Repository interface is minimal; grouping is a separate interface.
- **D** — `MainWindowViewModel` depends on `IPhotoRepository` and `IGroupingService` injected via constructor.

### MVVM Wiring

```
MainWindowViewModel
  ├── FilterPanelViewModel  → IPhotoRepository
  │     └── InboxViewModel  → IPhotoRepository
  ├── GalleryViewModel      → IPhotoRepository + IGroupingService
  └── DetailPanelViewModel  → IPhotoRepository

Events (cross-VM):
  Gallery.PhotoSelected       → DetailPanel.SelectPhotoAsync()
  FilterPanel.FilterChanged   → Gallery.LoadAsync()
  DetailPanel.PhotoCataloged  → Inbox.LoadAsync()
```

### Replacing the Data Layer

Swap `DesignTimePhotoRepository` for a real implementation:

```csharp
// In App.axaml.cs or a DI container:
IPhotoRepository repo = new SqlitePhotoRepository("catalog.db");
desktop.MainWindow = new MainWindow
{
    DataContext = new MainWindowViewModel(repo, new GroupingService())
};
```

## Extending

- **Add map view**: add `MapViewModel`, new tab in top nav, bind to `GpsLocation` on photos
- **Batch import**: add `ImportService : IImportService`, wire to a toolbar button
- **Real thumbnails**: in `PhotoThumbnailControl.axaml.cs`, replace gradient with `Bitmap` loaded async from `photo.FilePath`
- **SQLite persistence**: implement `IPhotoRepository` using `Microsoft.Data.Sqlite`
