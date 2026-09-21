using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using NextFrame.ViewModels;
using NextFrame.Views;
using System;

namespace NextFrame.CustomLogic;

public sealed class SplashScreen : ISplashScreen, IProgressReporter
{
    private readonly SplashViewModel viewModel = new SplashViewModel();
    private readonly IClassicDesktopStyleApplicationLifetime desktop;
    private readonly Func<Window> factory;
    private readonly Splash view;

    public SplashScreen(IClassicDesktopStyleApplicationLifetime desktop, Func<Window> factory)
    {
        this.desktop = desktop;
        this.factory = factory;
        view = new Splash
        {
            DataContext = viewModel,
        };
    }

    public void Show()
    {
        desktop.MainWindow = view;
        view.Show();
    }

    public IProgressReporter GetProgressReporter()
    {
        return this;
    }

    public void SetIndeterminate()
    {
        Dispatcher.UIThread.Post(() =>
        {
            viewModel.IsIndeterminate = true;
        });
    }

    public void ReportProgress(int progress, string message)
    {
        Dispatcher.UIThread.Post(() =>
        {
            viewModel.IsIndeterminate = false;
            viewModel.Progress = progress;
            viewModel.StatusMessage = message;
        });
    }

    public void Dispose()
    {
        SwitchToMainWindow();
        view.Close();
    }

    private void SwitchToMainWindow()
    {
        var mainWindow = factory();
        desktop.MainWindow = mainWindow;
        mainWindow.Show();
    }
}
