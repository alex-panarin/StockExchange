using StockExchangeWpfApp.ViewItems;
using System.Threading.Tasks;

namespace StockExchangeWpfApp.ViewModels
{
    public interface IChartViewModel
    {
        Task LoadAsync(ChartViewItem item);
        void SetTickerItems(int items);
    }
}