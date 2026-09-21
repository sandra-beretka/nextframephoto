using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NextFrame.CustomLogic;
using NextFrame.ViewModels;
using NextFrame.Views;
using System.Threading.Tasks;

namespace NextFrame;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            using (ISplashScreen screen = new SplashScreen(desktop, CreateMainWindow))
            {
                screen.Show();

                IProgressReporter reporter = screen.GetProgressReporter();
                await Task.Run(async () => await Initialize(reporter));
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static Window CreateMainWindow()
    {
        return new MainWindow
        {
            DataContext = new MainViewModel(),
        };
    }

    private static async Task Initialize(IProgressReporter reporter)
    {
        reporter.ReportProgress(0, "Loading configurations...");
        await Task.Delay(1000);
        reporter.ReportProgress(30, "Loading database...");
        await Task.Delay(1000);
        reporter.ReportProgress(60, "Loading thumbnails...");
        await Task.Delay(1000);
        reporter.ReportProgress(100, "DONE");
    }
}