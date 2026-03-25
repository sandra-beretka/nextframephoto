using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using ImageMagick;
using SQLiteAdapter.Models;


internal static class Program
{
    public static Dictionary<string, string> ExifMetadata = new Dictionary<string, string>();

    [SupportedOSPlatform("windows")]
    static void Main()
    {
        //string root = @"E:\Sanyi\Pictures\Mobilrol\Camera";
        string root = @"E:\Sanyi\Pictures";
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

        List<ExifMetadata> metadata = new List<ExifMetadata>();
        foreach (var item in ExifMetadata)
        {
            metadata.Add(new ExifMetadata { Tag = item.Key, Type = item.Value });
        }

        sw.Restart();
        NextframephotoContext context = new NextframephotoContext();
        context.Picture.AddRange(images);
        int changeCount = context.SaveChanges();
        sw.Stop();

        Console.WriteLine($"Metadata inserted in: {sw.Elapsed}");
        Console.ReadLine();
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
            //var thumb = profile?.CreateThumbnail();
            if (profile == null)
            {
                return retVal;
            }

            AddTypes(profile.Values);
        }
        catch
        {
            Console.WriteLine($"Error loading image: {imagePath}");
        }

        //retVal.FileCreationTime = File.GetCreationTimeUtc(imagePath);
        //retVal.Metadata = ImageMetadataReader.ReadMetadata(imagePath);
        return retVal;
    }

    private static void AddTypes(IReadOnlyList<IExifValue> values)
    {
        foreach (var item in values)
        {
            if (!ExifMetadata.ContainsKey(item.Tag.ToString()))
            {
                ExifMetadata.Add(item.Tag.ToString(), item.DataType.ToString());
            }
        }
    }
}