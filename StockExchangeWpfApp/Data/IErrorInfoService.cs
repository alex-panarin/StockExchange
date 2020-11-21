using System;

namespace StockExchangeWpfApp.Data
{
    public interface IErrorInfoService
    {
        void ShowError(Exception x);
    }
}