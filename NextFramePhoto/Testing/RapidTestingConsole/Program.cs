using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;
using ImageMagick;
using ImageMagick.Drawing;
using ImageMagick.Formats;
using MetadataExtractor;
using SQLiteAdapter.Models;


internal static class Program
{
    [SupportedOSPlatform("windows")]
    static void Main()
    {
        //string root = @"E:\Sanyi\Pictures\Mobilrol\Camera";
        string root = @"E:\Sanyi\Pictures\Nikon D70s";
        string[] files = System.IO.Directory.GetFiles(root, "*.jpg", SearchOption.AllDirectories);

        Console.WriteLine($"Images total: {files.Length}");

        Stopwatch sw = Stopwatch.StartNew();
        List<Picture> images = new List<Picture>();
        foreach (var item in files)
        {
            Picture info = ParseImage(item);
            images.Add(info);
        }

        sw.Stop();
        Console.WriteLine($"Thumbnails loaded in: {sw.Elapsed}");

        sw.Restart();
        NextframephotoContext context = new NextframephotoContext();
        context.Pictures.AddRange(images);
        int changeCount = context.SaveChanges();
        sw.Stop();

        Console.WriteLine($"Metadata inserted in: {sw.Elapsed}");
    }

    [SupportedOSPlatform("windows")]
    private static Picture ParseImage(string imagePath)
    {
        Picture retVal = new Picture
        {
            Path = imagePath
        };

        try
        {
            var image2 = new MagickImage();
            image2.Ping(imagePath, null);
            var profile = image2.GetExifProfile();
            var thumb = profile?.CreateThumbnail();
        }
        catch
        {
            Console.WriteLine($"Error loading image: {imagePath}");
        }

        //retVal.FileCreationTime = File.GetCreationTimeUtc(imagePath);
        //retVal.Metadata = ImageMetadataReader.ReadMetadata(imagePath);
        return retVal;
    }
}