using Prism.Commands;
using Prism.Events;
using StockExchangeDataModel.Models;
using StockExchangeWpfApp.ViewItems.Events;
using System.ComponentModel;
using System.Windows.Input;

namespace StockExchangeWpfApp.ViewItems
{
    public class ChartViewItem : ValidatedItem<ChartSetting>
    {
        private  readonly IEventAggregator _eventAggregator;
        
        public ChartViewItem(ChartSetting model, IEventAggregator  eventAggregator) 
            : base(model)
        {
            _eventAggregator = eventAggregator;

            ItemClick = new DelegateCommand(() =>
            {
                _eventAggregator.GetEvent<ChartViewItemEvent>().Publish(this);
            });

        }
        public ICommand ItemClick { get; private set; }
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        [TypeConverter(typeof(EnumConverter))]
        public ChartType ChartType
        {
            get { return GetValue<ChartType>(); }
            set { SetValue(value); }
        }
        public string AxisXTitle
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public string AxisYTitle
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public string Title
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public int Id
        {
            get { return GetValue<int>(); }
        }

    }
}
