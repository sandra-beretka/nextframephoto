using Avalonia.Media;
using Folio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Folio.Services;

/// <summary>
/// In-memory repository with realistic seed data.
/// Used by the XAML design previewer and unit tests.
/// Replace with a SQLite or file-system implementation for production.
/// </summary>
public sealed class DesignTimePhotoRepository : IPhotoRepository
{
    private readonly List<Device> _devices;
    private readonly List<Photo> _photos;

    public DesignTimePhotoRepository()
    {
        _devices = BuildDevices();
        _photos  = BuildPhotos(_devices);
    }

    public Task<IReadOnlyList<Device>> GetAllDevicesAsync() =>
        Task.FromResult<IReadOnlyList<Device>>(_devices);

    public Task<IReadOnlyList<Photo>> GetAllPhotosAsync() =>
        Task.FromResult<IReadOnlyList<Photo>>(_photos);

    public Task<IReadOnlyList<Photo>> GetUncatalogedPhotosAsync() =>
        Task.FromResult<IReadOnlyList<Photo>>(
            _photos.Where(p => !p.IsCataloged).ToList());

    public Task<IReadOnlyList<Photo>> GetPhotosByDeviceAsync(Guid deviceId) =>
        Task.FromResult<IReadOnlyList<Photo>>(
            _photos.Where(p => p.DeviceId == deviceId).ToList());

    public Task MarkCatalogedAsync(Guid photoId)
    {
        var photo = _photos.FirstOrDefault(p => p.Id == photoId);
        if (photo is not null)
            photo.IsCataloged = true;
        return Task.CompletedTask;
    }

    public Task SetStarredAsync(Guid photoId, bool starred)
    {
        var photo = _photos.FirstOrDefault(p => p.Id == photoId);
        if (photo is not null)
            photo.IsStarred = starred;
        return Task.CompletedTask;
    }

    // ── Seed data ────────────────────────────────────────────────────────────

    private static List<Device> BuildDevices() =>
    [
        new Device { Name = "Sony A7 IV",           Owner = "Martin",  Type = DeviceType.Camera,       AccentColor = Color.Parse("#7ec8a0"), PhotoCount = 12480 },
        new Device { Name = "iPhone 15 Pro",         Owner = "Anna",    Type = DeviceType.Phone,        AccentColor = Color.Parse("#80b0ff"), PhotoCount = 18340 },
        new Device { Name = "Pixel 8",               Owner = "Martin",  Type = DeviceType.Phone,        AccentColor = Color.Parse("#ffcc80"), PhotoCount = 9200  },
        new Device { Name = "GoPro Hero 12",         Owner = "Family",  Type = DeviceType.ActionCamera, AccentColor = Color.Parse("#d08080"), PhotoCount = 4100  },
        new Device { Name = "iPad",                  Owner = "Kids",    Type = DeviceType.Tablet,       AccentColor = Color.Parse("#b080d0"), PhotoCount = 2830, IsEnabled = false },
    ];

    private static List<Photo> BuildPhotos(List<Device> devices)
    {
        var photos = new List<Photo>();
        var sony   = devices[0];
        var iphone = devices[1];
        var pixel  = devices[2];

        // Recent uncataloged batch
        foreach (var i in Enumerable.Range(0, 12))
        {
            var device = i % 3 == 0 ? sony : i % 3 == 1 ? iphone : pixel;
            photos.Add(MakePhoto(device, new DateTime(2025, 3, 1).AddDays(-i), cataloged: false,
                file: $"DSC_{9000 + i:D4}.ARW", camera: device.Name,
                shutter: 0.002, aperture: 2.8, iso: 400));
        }

        // November 2024 — Prague trip
        foreach (var i in Enumerable.Range(0, 18))
        {
            var device = i % 3 == 0 ? sony : i % 3 == 1 ? iphone : pixel;
            photos.Add(MakePhoto(device, new DateTime(2024, 11, 27).AddHours(i * 4), cataloged: true,
                file: $"DSC_{4800 + i:D4}.ARW", camera: device.Name,
                shutter: 0.004, aperture: 4.0, iso: 800,
                location: "Prague, CZ"));
        }

        // October 2024
        foreach (var i in Enumerable.Range(0, 8))
        {
            var device = i % 2 == 0 ? iphone : pixel;
            photos.Add(MakePhoto(device, new DateTime(2024, 10, 15).AddHours(i * 5), cataloged: true,
                file: $"IMG_{3200 + i:D4}.HEIC", camera: device.Name,
                shutter: 0.001, aperture: 1.8, iso: 200));
        }

        // September 2024
        foreach (var i in Enumerable.Range(0, 10))
        {
            photos.Add(MakePhoto(sony, new DateTime(2024, 9, 10).AddHours(i * 6), cataloged: true,
                file: $"DSC_{3800 + i:D4}.ARW", camera: sony.Name,
                shutter: 0.002, aperture: 5.6, iso: 100,
                location: "Vienna, AT"));
        }

        return photos;
    }

    private static Photo MakePhoto(
        Device device, DateTime date, bool cataloged,
        string file, string camera,
        double shutter, double aperture, int iso,
        string? location = null) => new()
    {
        FilePath     = $"/photos/{date:yyyy/MM}/{file}",
        FileName     = file,
        DateTaken    = date,
        DeviceId     = device.Id,
        CameraModel  = camera,
        LensModel    = device.Type == DeviceType.Camera ? "FE 24-70mm f/2.8 GM" : null,
        ShutterSpeed = shutter,
        Aperture     = aperture,
        Iso          = iso,
        WidthPx      = 7008,
        HeightPx     = 4672,
        GpsLocation  = location,
        IsCataloged  = cataloged,
        Status       = cataloged ? CatalogStatus.Done : CatalogStatus.Uncataloged
    };
}
