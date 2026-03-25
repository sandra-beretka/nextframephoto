using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Folio.ViewModels;
using System;
using System.Collections.Generic;

namespace Folio.Views.Controls;

/// <summary>
/// Mini calendar that renders days for the currently-displayed month.
/// Days with photos show a small amber dot indicator.
/// </summary>
public partial class MiniCalendarControl : UserControl
{
    // Dummy set of "has-photo" days for design preview.
    // In production these would come from the repository via the VM.
    private static readonly HashSet<int> _photoDays = [2, 3, 9, 14, 15, 16, 17, 18, 28, 29, 30];

    public MiniCalendarControl()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is FilterPanelViewModel vm)
        {
            vm.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(FilterPanelViewModel.CalendarMonth))
                    RebuildDays(vm.CalendarMonth);
            };
            RebuildDays(vm.CalendarMonth);
        }
    }

    private void RebuildDays(DateTime month)
    {
        var grid = this.FindControl<ItemsControl>("DaysGrid");
        if (grid is null) return;

        var items = new List<Control>();
        int firstDow = ((int)new DateTime(month.Year, month.Month, 1).DayOfWeek + 6) % 7; // Mon=0
        int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
        bool isCurrentMonth = month.Year == DateTime.Today.Year && month.Month == DateTime.Today.Month;

        // Empty cells before first day
        for (int i = 0; i < firstDow; i++)
            items.Add(new Border());

        for (int day = 1; day <= daysInMonth; day++)
        {
            bool isToday  = isCurrentMonth && day == DateTime.Today.Day;
            bool hasPhoto = _photoDays.Contains(day);
            items.Add(BuildDayCell(day, isToday, hasPhoto));
        }

        grid.ItemsSource = items;
    }

    private static Control BuildDayCell(int day, bool isToday, bool hasPhoto)
    {
        var bg   = isToday  ? App.Current?.FindResource("Brush.Accent") as IBrush
                            : Brushes.Transparent;
        var fg   = isToday  ? (IBrush)Brushes.Black
                 : hasPhoto ? App.Current?.FindResource("Brush.Text")   as IBrush ?? Brushes.White
                            : App.Current?.FindResource("Brush.Text2")  as IBrush ?? Brushes.Gray;

        var panel = new Panel();

        var label = new TextBlock
        {
            Text = day.ToString(),
            FontSize = 10,
            Foreground = fg,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment   = Avalonia.Layout.VerticalAlignment.Center
        };
        panel.Children.Add(label);

        if (hasPhoto && !isToday)
        {
            var dot = new Ellipse
            {
                Width  = 3, Height = 3,
                Fill   = App.Current?.FindResource("Brush.Accent") as IBrush ?? Brushes.Orange,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment   = Avalonia.Layout.VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 2),
            };
            panel.Children.Add(dot);
        }

        return new Border
        {
            Background    = bg,
            CornerRadius  = new CornerRadius(4),
            Child         = panel,
            Cursor        = Avalonia.Input.Cursor.Default,
            MinHeight     = 22,
            Padding       = new Thickness(2)
        };
    }
}
