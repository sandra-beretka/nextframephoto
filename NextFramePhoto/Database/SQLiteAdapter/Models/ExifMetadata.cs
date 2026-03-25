using System;
using System.Collections.Generic;

namespace SQLiteAdapter.Models;

public partial class ExifMetadata
{
    public string? Tag { get; set; }

    public string? Type { get; set; }
}
