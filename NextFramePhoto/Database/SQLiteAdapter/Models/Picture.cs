using System;
using System.Collections.Generic;

namespace SQLiteAdapter.Models;

public partial class Picture
{
    public string Path { get; set; } = null!;

    public int Flags { get; set; }
}
