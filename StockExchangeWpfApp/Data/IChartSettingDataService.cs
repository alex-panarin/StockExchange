using StockExchangeDataModel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchangeWpfApp.Data
{
    public interface IChartSettingDataService
    {
        Task<IEnumerable<ChartSetting>> LoadAsync();
        Task DeleteAsync(ChartSetting model);
        Task SaveAsync(ChartSetting model);
    }
}
