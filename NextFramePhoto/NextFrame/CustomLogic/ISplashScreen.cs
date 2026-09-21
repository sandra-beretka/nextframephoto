using System;

namespace NextFrame.CustomLogic;

public interface ISplashScreen : IDisposable
{
    void Show();

    IProgressReporter GetProgressReporter();
}
