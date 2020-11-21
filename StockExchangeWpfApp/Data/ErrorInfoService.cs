using System;
using System.Windows;

namespace StockExchangeWpfApp.Data
{
    public class ErrorInfoService : IErrorInfoService
    {
        public void ShowError(Exception x)
        {
            MessageBox.Show(x.ToString(), "Error");
        }
    }
}
