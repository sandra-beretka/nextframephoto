using Avalonia;
using Avalonia.Controls;
using NextFrame.ViewModels;

namespace NextFrame.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }

    private void Window_Resized(object? sender, WindowResizedEventArgs e)
    {
        var screen = Screens?.ScreenFromWindow(this);
        var handle = screen?.TryGetPlatformHandle();
    }

    private void ExitFullScreenButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }

    private void FilmstripToggle_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainViewModel context = (MainViewModel)DataContext!;
        context.FilmStripVisible ^= true;
    }
}