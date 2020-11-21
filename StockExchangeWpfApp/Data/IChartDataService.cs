using StockExchangeDataModel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchangeWpfApp.Data
{
    public interface IChartDataService
    {
        Task<IEnumerable<ChartLookup>> GetLookups(ChartSetting file);
        //Task<ChartLookup> GetLookup();
    }
}
