using StockExchangeWpfApp.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace StockExchangeWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();

            DataContext = _viewModel = viewModel;
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Dispatcher = this.Dispatcher;
            _viewModel.LoadFiles();
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _viewModel.Dispose();
        }
    }
}
