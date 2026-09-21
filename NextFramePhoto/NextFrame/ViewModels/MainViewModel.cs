using CommunityToolkit.Mvvm.ComponentModel;

namespace NextFrame.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Greeting { get; set; } = "Welcome to Avalonia!";

    [ObservableProperty]
    public partial bool FilmStripVisible { get; set; } = true;
}
