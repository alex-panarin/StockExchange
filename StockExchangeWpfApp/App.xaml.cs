using Autofac;
using System;
using System.Windows;

namespace StockExchangeWpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;

            var container = Bootstraper.Register();
            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected thread error occured."

            + Environment.NewLine + e.Exception.Message, "Unexpected thread error");

            e.Handled = true;
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected error occured. Please inform the admin."

            + Environment.NewLine + e.Exception.Message, "Unexpected error");

            e.Handled = true;
        }
    }
    
}
