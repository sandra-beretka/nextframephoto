using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using AvaloniaApplication1.ViewModels;
using ImageMagick;
using SkiaSharp;

namespace AvaloniaApplication1.Views
{
    public partial class MainWindow : Window
    {
        SKBitmap sourceBitmap;
        float rotationAngle = 0f;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClickHandler(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // Update rotation state and request repaint. Actual drawing happens in the PaintSurface handler.
            if (sourceBitmap == null)
                return;

            message.Text = "Saci Maci";
            rotationAngle = (rotationAngle + 90f) % 360f;
            skiaCanvasView?.InvalidateSurface();
        }

        private void Window_Opened(object? sender, System.EventArgs e)
        {
            // Ensure we have a view model to load the image from
            var vm = DataContext as MainWindowViewModel;
            if (vm == null)
            {
                vm = new MainWindowViewModel();
                DataContext = vm;
            }

            try
            {
                vm.OnOpening();
            }
            catch (Exception ex)
            {
                message.Text = $"Error opening image: {ex.Message}";
                return;
            }

            // Convert and cache the source bitmap once to avoid repeated conversions
            try
            {
                sourceBitmap = ToSKBitmap(vm.Image);
            }
            catch (Exception ex)
            {
                message.Text = $"Error converting image: {ex.Message}";
                return;
            }

            // Request the view to repaint so the SKCanvasView will draw the source bitmap
            skiaCanvasView?.InvalidateVisual();
        }

        public static SKBitmap ToSKBitmap(MagickImage magickImage)
        {
            // 1. Ensure the MagickImage is in a format Skia understands (8-bit RGBA)
            // This handles the conversion from CMYK, Grayscale, or 16-bit to 8-bit sRGB.
            magickImage.Format = MagickFormat.Rgba;
            magickImage.Depth = 8;

            // 2. Extract the raw pixels as a byte array
            // 'Rgba' mapping ensures the byte order matches SKColorType.Rgba8888
            byte[] pixels = magickImage.ToByteArray(MagickFormat.Rgba);

            // 3. Create the SKBitmap and install the pixels
            var info = new SKImageInfo((int)magickImage.Width, (int)magickImage.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            var bitmap = new SKBitmap();

            // We use a pinned handle or simply copy the bytes into the bitmap's address space
            var handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            bitmap.InstallPixels(info, handle.AddrOfPinnedObject(), info.RowBytes, (addr, ctx) =>
            {
                handle.Free(); // Release the pin once Skia is done with the buffer
            });

            return bitmap;
        }

        private void SKCanvasView_PaintSurface(object? sender, Avalonia.Labs.Controls.SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            if (sourceBitmap == null)
                return;

            // Compute center of the surface (in pixels) and scale to fit the view
            float sw = e.Info.Width;
            float sh = e.Info.Height;

            float cx = sw / 2f;
            float cy = sh / 2f;

            // Determine a uniform scale so the image fits inside the view
            float scale = Math.Min(sw / (float)sourceBitmap.Width, sh / (float)sourceBitmap.Height);
            if (scale <= 0)
                scale = 1f;

            float dw = sourceBitmap.Width * scale;
            float dh = sourceBitmap.Height * scale;

            // Draw the source bitmap centered and rotated by rotationAngle
            canvas.Save();
            canvas.Translate(cx, cy);
            canvas.RotateDegrees(rotationAngle);

            var dest = new SKRect(-dw / 2f, -dh / 2f, dw / 2f, dh / 2f);
            canvas.DrawBitmap(sourceBitmap, dest);

            canvas.Restore();
        }

        private void Slider_ValueChanged(object? sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            rotationAngle = (float)e.NewValue;
            skiaCanvasView?.InvalidateSurface();
        }
    }
}