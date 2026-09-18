using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;
using NextFrame.ViewModels;

namespace NextFrame.Views
{
    public partial class FilmStrip : UserControl
    {
        public FilmStrip()
        {
            InitializeComponent();
            DataContext = new FilmStripViewModel();
        }

        private void FilmstripToggle_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            FilmStripViewModel context = (FilmStripViewModel)DataContext!;
            context.FilmStripVisible ^= true;
        }
    }
}