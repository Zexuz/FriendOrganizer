using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FriendOrganizer.UI.ViewModel;

namespace FriendOrganizer.UI.Wrapper
{
    public class NotifyDataErrorInfoBase:ViewModelBase, INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> _errorsByProprtyName = new Dictionary<string, List<string>>();

        public bool HasErrors => _errorsByProprtyName.Any();


        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByProprtyName.ContainsKey(propertyName)
                ? _errorsByProprtyName[propertyName]
                : null;
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            base.OnPropertyChanged(nameof(HasErrors));
        }

        protected void AddError(string propertyName, string error)
        {
            if (!_errorsByProprtyName.ContainsKey(propertyName))
            {
                _errorsByProprtyName[propertyName] = new List<string>();
            }
            if (!_errorsByProprtyName[propertyName].Contains(error))
            {
                _errorsByProprtyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        protected void ClearError(string propertyName)
        {
            if (_errorsByProprtyName.ContainsKey(propertyName))
            {
                _errorsByProprtyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}