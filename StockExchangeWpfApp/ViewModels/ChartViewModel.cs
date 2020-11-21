using LiveCharts;
using LiveCharts.Wpf;
using StockExchangeComon.ViewModels;
using StockExchangeDataModel.Models;
using StockExchangeWpfApp.Data;
using StockExchangeWpfApp.ViewItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;

namespace StockExchangeWpfApp.ViewModels
{
    public class ChartViewModel : BaseViewModel<ChartLookup>, IChartViewModel
    {
        private readonly IChartDataService _service;
        private readonly IErrorInfoService _errorInfo;
        private ObservableCollection<string> _labels;
        private int _ItemsCount;
        private string _axisX;
        private string _axisY;
        private Timer _timer;
        public ChartViewModel(IChartDataService service, IErrorInfoService errorInfo)
        {
            _service = service;
            _errorInfo = errorInfo;
            Series = new SeriesCollection();
            Labels = new ObservableCollection<string>();

            Formatter = (value) => $"{value} см";
        }
        public async Task LoadAsync(ChartViewItem item)
        {
            try
            {
                using (var col = Items.LockChangedEvent())
                {

                    StopTicker();

                    CreateSeries(item);

                    Items.Clear();

                    IEnumerable<ChartLookup> values = await _service.GetLookups(item.Model);

                    foreach (var val in values)
                    {
                        Series[0].Values.Add(val.Value);
                        Labels.Add(val.Date.ToShortDateString());
                        Items.Add(val);
                    }
                }
                
                if (Items.Count > _ItemsCount)
                {
                    ProcessTicker();
                }

            }
            catch (Exception x)
            {
                _errorInfo.ShowError(x);
            }
        }
        public void SetTickerItems(int items)
        {
            _ItemsCount = items;
        }
        public SeriesCollection Series { get; private set; }
        public ObservableCollection<string> Labels
        {
            get { return _labels; }
            set {
                _labels = value;
                OnPropertyChanged();
            }
        }
        public Func<int, string> Formatter { get; set; }
        public string AxisX
        {
            get { return _axisX; }
            set 
            {
                _axisX = value;
                OnPropertyChanged();
            }
        }
        public string AxisY
        {
            get { return _axisY; }
            set
            {
                _axisY = value;
                OnPropertyChanged();
            }
        }
        private void CreateSeries(ChartViewItem item)
        {
            Series.Clear();
            Labels.Clear();

            AxisX = item.AxisXTitle;
            AxisY = item.AxisYTitle;

            switch (item.ChartType)
            {
                case ChartType.Column:
                    {
                        Series.Add(
                                new ColumnSeries
                                {
                                    Fill = Brushes.Coral,
                                    Title = item.Title,
                                    Values = new ChartValues<int>()
                                }); 
                        break;
                    }
                case ChartType.Line:
                    {
                        Series.Add(
                                new LineSeries
                                {
                                    Fill = Brushes.LightGreen,
                                    Title = item.Title,
                                    Values = new ChartValues<int>()
                                });

                        break;
                    }
                case ChartType.Scatter:
                    {
                        Series.Add(
                                new ScatterSeries
                                {
                                    Title = item.Title,
                                    Values = new ChartValues<int>()
                                });

                        break;
                    }
                default:
                    break;
            }
        }
        private void ProcessTicker()
        {
            Dispatcher d = Dispatcher.CurrentDispatcher;
            _timer = new Timer
             (
                 _ =>
                 {
                     d.Invoke(() =>
                     {
                         var item = Items[0];

                         Items.Remove(item);
                         Items.Add(item);
                     });

                 }, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(1)
             );
        }
        public void StopTicker()
        {
            if (_timer == null) return;
            
            _timer.Dispose();
            _timer = null;
            
        }
        public override void Dispose()
        {
            StopTicker();
            base.Dispose();
        }
    }
}
