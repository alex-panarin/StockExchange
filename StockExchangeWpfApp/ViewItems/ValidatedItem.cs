using StockExchangeComon.Wrapers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StockExchangeWpfApp.ViewItems
{
    public class ValidatedItem<TEntity> : BaseWraper<TEntity>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        protected ValidatedItem(TEntity model) 
            : base(model)
        {

        }
        public bool HasErrors => _errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ? _errorsByPropertyName[propertyName] : null;
        }
        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }
        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }
        protected override void SetValue<TValue>(TValue value, [CallerMemberName] string propertyName = null)
        {
            base.SetValue(value, propertyName);
            ValidatePropertyInternal(propertyName, value);
        }
        private void ValidatePropertyInternal(string propertyName, object currentValue)
        {
            ClearErrors(propertyName);
            // 1. Validate Data Annotations
            ValidateDataAnnotations(propertyName, currentValue);
            //2. Validate Custom errors
            ValidateCustomErrors(propertyName);
        }
        private void ValidateDataAnnotations(string propertyName, object currentValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(Model) { MemberName = propertyName };

            Validator.TryValidateProperty(currentValue, context, results);

            foreach (var item in results)
            {
                AddError(propertyName, item.ErrorMessage);
            }
        }
        private void ValidateCustomErrors(string propertyName)
        {
            var errors = ValidateProperty(propertyName);
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    AddError(propertyName, error);
                }
            }
        }
        private void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }
        private void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
       
    }
}
