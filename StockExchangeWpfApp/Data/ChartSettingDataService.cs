using Newtonsoft.Json;
using StockExchangeDataModel.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace StockExchangeWpfApp.Data
{
    public class ChartSettingDataService : IChartSettingDataService
    {
        private readonly IErrorInfoService _infoService;
        private readonly string connection;
        public ChartSettingDataService(IErrorInfoService infoService)
        {
            _infoService = infoService;
            connection = ConfigurationManager.ConnectionStrings["ChartSettings"].ConnectionString;
        }
        public async Task DeleteAsync(ChartSetting model)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var msg = await client.DeleteAsync($"{connection}/{model.Id}");
                    var md = JsonConvert.DeserializeObject<ChartSetting>(await msg.Content.ReadAsStringAsync());
                }
            }
            catch (Exception x)
            {
                _infoService.ShowError(x);
            }
        }
        public async Task<IEnumerable<ChartSetting>> LoadAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var msg = await client.GetStringAsync(connection);

                    return JsonConvert.DeserializeObject<List<ChartSetting>>(msg);
                }
            }
            catch (Exception x)
            {
                _infoService.ShowError(x);
            }

            return await Task.FromResult(new ChartSetting[] { new ChartSetting { Id = 1, Title = "Some chart from test", Name = "Line Chart", ChartType = ChartType.Line, AxisXTitle="Date", AxisYTitle = "Value"} });
        }
        public async Task SaveAsync(ChartSetting model)
        {
            try
            {
                StringContent sc = new StringContent(JsonConvert.SerializeObject(model));
                sc.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                using (HttpClient client = new HttpClient())
                {
                    var msg = model.Id == 0 ?
                        await client.PostAsync($"{connection}", sc) :
                        await client.PutAsync($"{connection}/{model.Id}", sc);

                    if (msg.IsSuccessStatusCode)
                    {
                        var md = JsonConvert.DeserializeObject<ChartSetting>(await msg.Content.ReadAsStringAsync());
                        model.Id = md.Id;
                    }

                }
            }
            catch (Exception x)
            {
                _infoService.ShowError(x);
            }
        }
    }
}
