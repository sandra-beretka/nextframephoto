using ImageMagick;

namespace AvaloniaApplication1.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Greeting { get; } = "Welcome to Avalonia!";

        public MagickImage Image { get; set; }

        public void OnOpening()
        {
            const string path = @"E:\Sanyi\Pictures\Mobilrol\Camera\20200512_144837.jpg";

            Image = new MagickImage(path);
        }
    }
}
