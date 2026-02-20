using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SQLiteAdapter.Models;

public partial class NextframephotoContext : DbContext
{
    public NextframephotoContext()
    {
    }

    public NextframephotoContext(DbContextOptions<NextframephotoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Picture> Pictures { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=Data\\nextframephoto.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasKey(e => e.Path);

            entity.ToTable("PICTURE");

            entity.Property(e => e.Path).HasColumnName("PATH");
            entity.Property(e => e.Flags).HasColumnName("FLAGS");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
