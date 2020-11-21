using Autofac;
using Prism.Events;
using StockExchangeNotificationClient;
using StockExchangeWpfApp.Data;
using StockExchangeWpfApp.ViewModels;


namespace StockExchangeWpfApp
{
    public static class Bootstraper
    {
        public static IContainer Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<ErrorInfoService>().As<IErrorInfoService>().SingleInstance();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<NotificationServiceClient>().As<INotificationServiceClient>().SingleInstance();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<ChartViewModel>().As<IChartViewModel>();
            builder.RegisterType<ChartDataService>().As<IChartDataService>();
            builder.RegisterType<ChartSettingDataService>().As<IChartSettingDataService>();
            

            return builder.Build();
        }
    }
}
