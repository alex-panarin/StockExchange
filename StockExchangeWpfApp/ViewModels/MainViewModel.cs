using Prism.Commands;
using Prism.Events;
using StockExchangeComon.ViewModels;
using StockExchangeWpfApp.Data;
using StockExchangeDataModel.Models;
using StockExchangeWpfApp.ViewItems;
using StockExchangeWpfApp.ViewItems.Events;
using System;
using System.ComponentModel;
using System.Linq;
using StockExchangeNotificationClient;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Threading;
using StockExchangeNotification;

namespace StockExchangeWpfApp.ViewModels
{
    public class MainViewModel : BaseViewModel<ChartViewItem>  
    {
        private readonly IChartSettingDataService _service;
        private readonly IEventAggregator _eventAggregator;
        private readonly Func<IChartViewModel> _chartModel;
        private readonly INotificationServiceClient _notification;
        private ChartViewItem _selectedItem;
        private IChartViewModel _chartlViewModel;
        private bool _hasChanges;
        private bool _isDisposed;
        public MainViewModel(
            IChartSettingDataService service,
            INotificationServiceClient notification, 
            IEventAggregator eventAggregator, 
            Func<IChartViewModel> chartModel)
        {
            _service = service;
            _eventAggregator = eventAggregator;
            _chartModel = chartModel;
            _notification = notification;

            _eventAggregator.GetEvent<ChartViewItemEvent>().Subscribe(OnChartItemView);

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute, OnDeleteCanExecute);
            AddNewCommand = new DelegateCommand(OnAddNewExecute, OnAddNewCanExecute);

            _notification.OpenConnection(new Uri(ConfigurationManager.ConnectionStrings["NotificationRemote"].ConnectionString));

            _notification.EventMessage += OnNotificationRecieved;
        }

        public async void LoadFiles()
        {
            Items.Clear();

            var files = await _service.LoadAsync();
            
            ChartViewItem cvi = null;

            foreach (var file in files)
            {
                cvi = AddChartItem(file);
            }

            if(Items.Count == 0)
            {
                cvi = AddChartItem();
            }

            SelectedItem = cvi;
            
        }
        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges == value) return;

                _hasChanges = value;
                OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
                AddNewCommand.RaiseCanExecuteChanged();
            }
        }
        public ChartViewItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if(_selectedItem != null)
                {
                    _selectedItem.PropertyChanged -= (sender, e) => SelectedItemPropertyChanged(sender, e);  
                }

                _selectedItem = value;
                
                OnPropertyChanged();
                DeleteCommand.RaiseCanExecuteChanged();
                AddNewCommand.RaiseCanExecuteChanged();

                if (_selectedItem != null)
                {
                    _selectedItem.PropertyChanged += (sender, e) => SelectedItemPropertyChanged(sender, e);
                }
            }
        }
        public IChartViewModel ChartViewModel
        {
            get { return _chartlViewModel; }
            private set
            {
                _chartlViewModel = value;
                OnPropertyChanged();
            }
        }
        public Dispatcher Dispatcher { get; set; }

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }
        public DelegateCommand AddNewCommand { get; private set; }

        private void OnAddNewExecute()
        {
            SelectedItem = AddChartItem();
            HasChanges = true;
        }
        private async void OnDeleteExecute()
        {
            await _service.DeleteAsync(SelectedItem.Model);
            
            Items.Remove(SelectedItem);

            SelectedItem = null;
        }
        private async void OnSaveExecute()
        {
            await _service.SaveAsync(SelectedItem.Model);
            HasChanges = false;
        }
        private bool OnDeleteCanExecute()
        {
            return 
                SelectedItem != null &&
                SelectedItem.Id != 0;
        }
        private bool OnSaveCanExecute()
        {
            return 
                SelectedItem != null 
                && !SelectedItem.HasErrors
                && HasChanges;
        }
        private bool OnAddNewCanExecute()
        {
            return 
                SelectedItem != null 
                && !Items.Any(it => it.Id == 0);
        }
        private async void OnChartItemView(ChartViewItem item)
        {
            ChartViewModel = _chartModel();

            SelectedItem = item;

            await ChartViewModel.LoadAsync(item);
        }
        private ChartViewItem AddChartItem(ChartSetting cs = null)
        {
            var csv = cs == null ?
                new ChartViewItem(new ChartSetting { ChartType = ChartType.Column, Name = "Новый график", AxisXTitle="Ось X", AxisYTitle="Ось Y", Title ="Легенда"}, _eventAggregator) :
                new ChartViewItem(cs, _eventAggregator);

            Items.Add(csv);

            return csv;
        }
        private void SelectedItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(!HasChanges)
            {
                HasChanges = true;
            }

            if(e.PropertyName == nameof(SelectedItem.HasErrors))
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private void OnNotificationRecieved(INotificationServiceClient s, NotificationPayload message)
        {
            Dispatcher?.Invoke(() =>
            {

                switch (message.Notification)
                {
                    case Notifications.Error:
                        Debug.WriteLine(message);
                        break;
                    case Notifications.Connect:
                        Debug.WriteLine("CONNECTED");
                        break;
                    case Notifications.Disconnect:
                        Debug.WriteLine("DISCONNECTED");
                        break;
                    case Notifications.Notify:
                        ChartViewModel?.LoadAsync(SelectedItem);
                        break;
                }
            });
            
        }
        public override void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            
            _notification.CloseConnection();

            (ChartViewModel as IDisposable)?.Dispose();

            Task.Delay(1000);

            base.Dispose();
        }
    }
}
