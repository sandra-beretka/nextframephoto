using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
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
            // 1. Create and show the Splash Screen
            var splashVM = new SplashViewModel();
            var splash = new Splash
            {
                DataContext = splashVM,
            };

            desktop.MainWindow = splash;
            splash.Show();

            // 2. Perform initialization tasks on a background thread
            // Simulate loading configurations, databases, or API calls
            await Task.Run(async () => await Initialize(splashVM));

            // 3. Switch to the Main Window on the UI Thread
            var mainWindow = new MainWindow
            {
                DataContext = new MainViewModel(),
            };

            desktop.MainWindow = mainWindow;
            mainWindow.Show();
            splash.Close(); // Close the splash screen
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void UpdateInfo(SplashViewModel vm, int progress, string message, bool isIndeterminate=false)
    {
        Dispatcher.UIThread.Post(() =>
        {
            vm.IsIndeterminate = false;
            vm.Progress = progress;
            vm.StatusMessage = message;
        });
    }


    private async Task Initialize(SplashViewModel vm)
    {
        UpdateInfo(vm,0, "Loading configurations...");
        await Task.Delay(1000);
        UpdateInfo(vm, 30, "Loading database...");
        await Task.Delay(1000);
        UpdateInfo(vm, 60, "Loading thumbnails...");
        await Task.Delay(1000);
        UpdateInfo(vm, 100, "DONE");
    }
}