using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextFrame.ViewModels
{
    public partial class SplashViewModel : ViewModelBase
    {
        [ObservableProperty]
        public partial bool IsIndeterminate { get; set; } = true;

        [ObservableProperty]
        public partial int Progress { get; set; } = 0;

        [ObservableProperty]
        public partial string StatusMessage { get; set; } = "Loading...";
    }
}
