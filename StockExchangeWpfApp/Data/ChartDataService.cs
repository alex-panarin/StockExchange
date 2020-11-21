using Newtonsoft.Json;
using StockExchangeDataModel.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockExchangeWpfApp.Data
{
    public class ChartDataService : IChartDataService
    {
        private readonly IErrorInfoService _errorInfo;
        private readonly string connection;
     
        public ChartDataService(IErrorInfoService errorInfo)
        {
            _errorInfo = errorInfo;
            connection = ConfigurationManager.ConnectionStrings["ChartLookups"].ConnectionString;
        }
        public async Task<IEnumerable<ChartLookup>> GetLookups(ChartSetting item)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var msg = await client.GetStringAsync($"{connection}/Owner={item.Id}");

                    return JsonConvert.DeserializeObject<IEnumerable<ChartLookup>>(msg);
                }
            }
            catch (Exception x)
            {
                _errorInfo.ShowError(x);
            }


            return await Task.FromResult(new ChartLookup[]
            {
                new ChartLookup {Id = 1, Date = DateTime.Now - TimeSpan.FromDays(1),  Value = 250 },
                new ChartLookup {Id = 2, Date = DateTime.Now ,  Value = 350 },
                new ChartLookup {Id = 3, Date = DateTime.Now + TimeSpan.FromDays(1),  Value = 150 }
            });
        }
    }
}
