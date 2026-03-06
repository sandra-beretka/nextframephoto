using Avalonia.Controls;
using Folio.ViewModels;

namespace Folio.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnOpened(System.EventArgs e)
    {
        base.OnOpened(e);
        if (DataContext is MainWindowViewModel vm)
            vm.LoadCommand.Execute(null);
    }
}
