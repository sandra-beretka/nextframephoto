using CommunityToolkit.Mvvm.ComponentModel;

namespace NextFrame.ViewModels
{
    public partial class FilmStripViewModel : ViewModelBase
    {
        [ObservableProperty]
        public partial bool FilmStripVisible { get; set; } = true;
    }
}
