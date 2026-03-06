using Folio.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Folio.Services;

/// <summary>
/// Abstracts all photo data access. Implementations can be file-system,
/// SQLite, or in-memory (for design/testing).
/// </summary>
public interface IPhotoRepository
{
    Task<IReadOnlyList<Photo>> GetAllPhotosAsync();
    Task<IReadOnlyList<Photo>> GetUncatalogedPhotosAsync();
    Task<IReadOnlyList<Photo>> GetPhotosByDeviceAsync(Guid deviceId);
    Task<IReadOnlyList<Device>> GetAllDevicesAsync();
    Task MarkCatalogedAsync(Guid photoId);
    Task SetStarredAsync(Guid photoId, bool starred);
}
