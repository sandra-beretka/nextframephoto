using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SQLiteAdapter.Models;

public partial class NextframephotoContext : DbContext
{
    public NextframephotoContext(DbContextOptions<NextframephotoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ExifMetadata> ExifMetadata { get; set; }

    public virtual DbSet<Picture> Picture { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExifMetadata>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("EXIF_METADATA");

            entity.Property(e => e.Tag).HasColumnName("TAG");
            entity.Property(e => e.Type).HasColumnName("TYPE");
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.ToTable("PICTURE");

            entity.HasIndex(e => e.Path, "IX_PICTURE_PATH").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Flags).HasColumnName("FLAGS");
            entity.Property(e => e.Path).HasColumnName("PATH");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
