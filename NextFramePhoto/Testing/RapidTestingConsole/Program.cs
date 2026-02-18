using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Iptc;
using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.Formats.Xmp;
using System;
using System.Drawing;
using System.IO;

internal static class Program
{
    static void Main()
    {
        Console.WriteLine("Hello, World!");
        string imagePath = @"C:\Users\sando\OneDrive\Pictures\viber_image_2026-01-24_22-43-48-872.jpg";

        //Image Size
        using (var image = Image.FromFile(imagePath))
        {
            Console.WriteLine($"Image size: {image.Width}x{image.Height} pixel");
        }

        // Timestampja
        var fileInfo = new FileInfo(imagePath);
        Console.WriteLine($"Timestamp: {fileInfo.CreationTime}");

        // EXIF information
        var metadata = ImageMetadataReader.ReadMetadata(imagePath);

        foreach (var directory in metadata)
        {
            if (directory is ExifIfd0Directory exif)
            {
                Console.WriteLine($"EXIF: {exif.GetDescription(ExifDirectoryBase.TagModel)}");
            }

            if (directory is IptcDirectory iptc)
            {
                Console.WriteLine($"IPTC: {iptc.GetDescription(IptcDirectory.TagHeadline)}");
            }

            if (directory is XmpDirectory xmp)
            {
                Console.WriteLine($"XMP: {xmp.GetDescription(XmpDirectory.TagXmpValueCount)}");
            }
        }

        // JPEG comment 
        

    }
}