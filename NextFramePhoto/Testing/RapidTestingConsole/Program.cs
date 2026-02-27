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


internal static class Program
{
    private class ImageInfo
    {
        public ImageInfo(string path)
        {
            Path = path;
            Metadata = Array.Empty<MetadataExtractor.Directory>();
        }

        public string Path { get; }

        public int Width { get; set; }

        public int Height { get; set; }

        public DateTime FileCreationTime { get; set; }

        public IReadOnlyList<MetadataExtractor.Directory> Metadata { get; set; }
    }

    [SupportedOSPlatform("windows")]
    static void Main()
    {
        string root = @"E:\Sanyi\Pictures\Mobilrol\Camera";
        string[] files = System.IO.Directory.GetFiles(root, "*.jpg", SearchOption.TopDirectoryOnly);

        Stopwatch sw = Stopwatch.StartNew();
        List<ImageInfo> images = new List<ImageInfo>();
        foreach (var item in files)
        {
            ImageInfo info = ParseImage(item);
            images.Add(info);
        }

        sw.Stop();

        Console.WriteLine(sw.Elapsed);
    }

    [SupportedOSPlatform("windows")]
    private static ImageInfo ParseImage(string imagePath)
    {
        ImageInfo retVal = new ImageInfo(imagePath);

        var image2 = new MagickImage();
        image2.Ping(imagePath, null);
        var profile = image2.GetExifProfile();
        var thumb = profile?.CreateThumbnail();

        retVal.Width = (int?)thumb?.Width ?? 0;
        retVal.Height = (int?)thumb?.Height ?? 0;
        //retVal.FileCreationTime = File.GetCreationTimeUtc(imagePath);
        //retVal.Metadata = ImageMetadataReader.ReadMetadata(imagePath);
        return retVal;
    }
}