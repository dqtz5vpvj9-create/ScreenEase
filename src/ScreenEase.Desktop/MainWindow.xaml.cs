using System.Windows;
using System.Windows.Threading;

namespace ScreenEase.Desktop;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel = new();
    private readonly DispatcherTimer _timer = new()
    {
        Interval = TimeSpan.FromSeconds(1)
    };

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _viewModel;
        _timer.Tick += (_, _) => _viewModel.TickRestTimer();
        _timer.Start();
        Loaded += async (_, _) => await _viewModel.RefreshAsync();
    }
}
