using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NextFrame.ViewModels;

namespace NextFrame.Views
{
    public partial class SidePane : UserControl
    {
        public SidePane()
        {
            InitializeComponent();
            DataContext = new SidePaneViewModel();
        }
    }
}