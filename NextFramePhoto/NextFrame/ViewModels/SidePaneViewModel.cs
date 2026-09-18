using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextFrame.ViewModels
{
    public partial class SidePaneViewModel : ViewModelBase
    {
        [ObservableProperty]
        public partial bool IsPaneOpen { get; set; } = true;

        [RelayCommand]
        public void TogglePane() => IsPaneOpen = !IsPaneOpen;
    }
}
