using Avalonia.Controls;
using Folio.ViewModels;
using System;

namespace Folio.Views.Controls;

public partial class InboxPanelControl : UserControl
{
    public InboxPanelControl()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is InboxViewModel vm)
        {
            vm.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(InboxViewModel.ProgressPercent))
                    UpdateProgressBar(vm.ProgressPercent);
            };
            UpdateProgressBar(vm.ProgressPercent);
        }
    }

    private void UpdateProgressBar(double percent)
    {
        var bar = this.FindControl<Border>("ProgressBar");
        var track = bar?.Parent as Border;
        if (bar is null || track is null) return;

        // Set width as a fraction of the track
        bar.Width = track.Bounds.Width * percent;
    }

    protected override void OnSizeChanged(Avalonia.Controls.SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        if (DataContext is InboxViewModel vm)
            UpdateProgressBar(vm.ProgressPercent);
    }
}
