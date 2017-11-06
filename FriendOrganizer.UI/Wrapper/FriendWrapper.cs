using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FriendOrganizer.Model;
using FriendOrganizer.UI.ViewModel;

namespace FriendOrganizer.UI.Wrapper
{
    public class FriendWrapper : ViewModelBase, INotifyDataErrorInfo
    {
        public Friend Model { get; }
        public int Id => Model.Id;


        public string FirstName
        {
            get => Model.FirstName;
            set
            {
                Model.FirstName = value;
                OnPropertyChanged();
                ValidatePropery(nameof(FirstName));
            }
        }

        private void ValidatePropery(string propertyName)
        {
            ClearError(propertyName);
            switch (propertyName)
            {
                case nameof(FirstName):
                    if(string.Equals(FirstName,"Robot",StringComparison.OrdinalIgnoreCase))
                    {
                        AddError(propertyName,"Robots are note valid friends");
                    }
                    break;
            }
        }

        public string LastName
        {
            get => Model.LastName;
            set
            {
                Model.LastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => Model.Email;
            set
            {
                Model.Email = value;
                OnPropertyChanged();
            }
        }

        public FriendWrapper(Friend model)
        {
            Model = model;
        }


        private Dictionary<string, List<string>> _errorsByProprtyName = new Dictionary<string, List<string>>();

        public bool HasErrors => _errorsByProprtyName.Any();


        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByProprtyName.ContainsKey(propertyName)
                ? _errorsByProprtyName[propertyName]
                : null;
        }

        private void OnErrorChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void AddError(string propertyName, string error)
        {
            if (!_errorsByProprtyName.ContainsKey(propertyName))
            {
                _errorsByProprtyName[propertyName] = new List<string>();
            }
            if (!_errorsByProprtyName[propertyName].Contains(error))
            {
                _errorsByProprtyName[propertyName].Add(error);
                OnPropertyChanged();
            }
        }

        private void ClearError(string propertyName)
        {
            if (!_errorsByProprtyName.ContainsKey(propertyName))
            {
                _errorsByProprtyName.Remove(propertyName);
                OnPropertyChanged();
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}