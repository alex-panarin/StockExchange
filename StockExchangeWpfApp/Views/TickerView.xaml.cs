using StockExchangeWpfApp.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StockExchangeWpfApp.Views
{
    /// <summary>
    /// Interaction logic for TickerView.xaml
    /// </summary>
    public partial class TickerView : ItemsControl
    {
        private const int ItemWidth = 100;
        public TickerView()
        {
            InitializeComponent();
        }

        private void OnTickerViewLoaded(object sender, RoutedEventArgs e)
        {
            int items = (int)this.ActualWidth / ItemWidth;

            IChartViewModel model = DataContext as IChartViewModel;
            if (model == null) return;

            model.SetTickerItems(items);
        }
    }
}
