using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Collections.ObjectModel;

namespace AvaloniaApplication1.Views.Controls
{
    public partial class VerticalGridGalleryControl : UserControl
    {
        public static readonly StyledProperty<int> ItemsPerRowProperty =
            AvaloniaProperty.Register<VerticalGridGalleryControl, int>(nameof(ItemsPerRow), 4);

        public static readonly DirectProperty<VerticalGridGalleryControl, double> ItemSizeProperty =
            AvaloniaProperty.RegisterDirect<VerticalGridGalleryControl, double>(nameof(ItemSize), o => o.ItemSize, (o, v) => o.ItemSize = v);

        public static readonly StyledProperty<ObservableCollection<IBrush>> ColorsProperty =
            AvaloniaProperty.Register<VerticalGridGalleryControl, ObservableCollection<IBrush>>(nameof(Colors), new ObservableCollection<IBrush>());

        private double _itemSize;

        public VerticalGridGalleryControl()
        {
            AvaloniaXamlLoader.Load(this);
            Colors = new ObservableCollection<IBrush>();
            this.SizeChanged += (_, __) => Recalculate();
        }

        public int ItemsPerRow
        {
            get => GetValue(ItemsPerRowProperty);
            set => SetValue(ItemsPerRowProperty, value);
        }

        public double ItemSize
        {
            get => _itemSize;
            set => SetAndRaise(ItemSizeProperty, ref _itemSize, value);
        }

        public ObservableCollection<IBrush> Colors
        {
            get => GetValue(ColorsProperty);
            set => SetValue(ColorsProperty, value);
        }

        public ObservableCollection<IBrush> PaddedColors { get; private set; } = new ObservableCollection<IBrush>();

        private void Recalculate()
        {
            var width = Math.Max(0.0, Bounds.Width);
            if (ItemsPerRow <= 0) return;

            ItemSize = width / ItemsPerRow - 2; // margin compensation

            var count = Colors?.Count ?? 0;
            var rows = (count + ItemsPerRow - 1) / ItemsPerRow;
            var total = Math.Max(ItemsPerRow, rows * ItemsPerRow);

            PaddedColors.Clear();
            if (Colors != null)
            {
                foreach (var c in Colors)
                    PaddedColors.Add(c);
            }

            for (int i = PaddedColors.Count; i < total; i++)
                PaddedColors.Add(Brushes.Black);

            var ic = this.FindControl<ItemsControl>("PART_VG_ItemsControl");
            if (ic != null)
            {
                ic.ItemsSource = PaddedColors;
            }
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == ColorsProperty)
            {
                if (change.OldValue != null && change.OldValue is ObservableCollection<IBrush> old)
                    old.CollectionChanged -= (_, __) => Recalculate();

                if (change.NewValue != null && change.NewValue is ObservableCollection<IBrush> nw)
                    nw.CollectionChanged += (_, __) => Recalculate();
            }

            if (change.Property == ItemsPerRowProperty || change.Property == ColorsProperty)
            {
                Recalculate();
            }
        }
    }
}
