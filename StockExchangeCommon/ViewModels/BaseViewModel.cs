using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StockExchangeComon.ViewModels
{
    public class BaseViewModel<TEntity> : INotifyPropertyChanged, IDisposable
    {
        public BaseViewModel()
        {
            Items = new BaseCollection<TEntity>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public virtual void Dispose()
        {
        }
        public BaseCollection<TEntity> Items { get; private set; }

    }
}
