namespace NextFrame.CustomLogic;

public interface IProgressReporter
{
    void SetIndeterminate();

    void ReportProgress(int progress, string message);
}
