using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositries;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Servicies;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private readonly IFriendReposetory _friendReposetory;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IProgramminLanguageLookupDataService _programminLanguageLookupDataService;
        private FriendWrapper _friend;
        private bool _hasChanges;
        private FriendPhoneNumberWrapper _selectedPhoneNumber;

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public ICommand AddPhoneNumberCommand { get; }
        public ICommand RemovePhoneNumberCommand { get; }

        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }
        public ObservableCollection<FriendPhoneNumberWrapper> PhoneNumbers { get; }

        public FriendPhoneNumberWrapper SelectedPhoneNumber
        {
            get { return _selectedPhoneNumber; }
            set
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand) RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }


        public FriendWrapper Friend
        {
            get => _friend;
            private set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public bool HasChanges
        {
            get => _hasChanges;
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }


        public FriendDetailViewModel
        (
            IFriendReposetory friendReposetory,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IProgramminLanguageLookupDataService programminLanguageLookupDataService
        )
        {
            _friendReposetory = friendReposetory;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _programminLanguageLookupDataService = programminLanguageLookupDataService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);

            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecure);

            ProgrammingLanguages = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<FriendPhoneNumberWrapper>();
        }

        public async Task LoadAsync(int? friendId)
        {
            var friend = friendId.HasValue
                ? await _friendReposetory.GetByIdAsync(friendId.Value)
                : CreateNewFriend();


            InitializeFriend(friend);
            InitializeFriendPhoneNumbers(friend.PhoneNumbers);


            await LoadProgrammingLangueagesLookupAsync();
        }

        private void InitializeFriendPhoneNumbers(ICollection<FriendPhoneNumber> friendPhoneNumbers)
        {
            foreach (var wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            }
            PhoneNumbers.Clear();

            foreach (var phoneNumber in friendPhoneNumbers)
            {
                var wrapper = new FriendPhoneNumberWrapper(phoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            }
        }

        private void FriendPhoneNumberWrapper_PropertyChanged(object o, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _friendReposetory.HasChanges();
            }
            if (e.PropertyName == nameof(FriendPhoneNumberWrapper.HasErrors))
            {
                ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void InitializeFriend(Friend friend)
        {
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (sender, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _friendReposetory.HasChanges();
                }
                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            if (Friend.Id == 0)
            {
                Friend.FirstName = "";
            }
        }

        private async Task LoadProgrammingLangueagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem {DisplayMember = " - "});
            var lookup = await _programminLanguageLookupDataService.GetProgrammingLanguageLookupAsync();
            foreach (var lookupItem in lookup)
            {
                ProgrammingLanguages.Add(lookupItem);
            }
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            _friendReposetory.Add(friend);
            return friend;
        }

        private async void OnSaveExecute()
        {
            await _friendReposetory.SaveAsync();
            HasChanges = _friendReposetory.HasChanges();
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(
                new AfterSavedEventArgs
                {
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}",
                    Id = Friend.Id,
                    ViewModelName = nameof(FriendDetailViewModel)
                }
            );
        }

        private bool OnSaveCanExecute()
        {
            return Friend != null
                   && !Friend.HasErrors
                   && PhoneNumbers.All(pn => !pn.HasErrors)
                   && HasChanges;
        }

        private async void OnDeleteExecute()
        {
            var res = _messageDialogService.ShowOkCancelDialog($"Do you readly want to delete the friend {Friend.FirstName} {Friend.LastName}", "Question");
            if (res == MessageDialogResult.Ok)
            {
                _friendReposetory.Remove(Friend.Model);
                await _friendReposetory.SaveAsync();
                _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(new AfterDetailDeletedEventArgs
                {
                    Id = Friend.Id,
                    ViewModelName = nameof(FriendDetailViewModel)
                });
            }
        }

        private bool OnRemovePhoneNumberCanExecure()
        {
            return SelectedPhoneNumber != null;
        }

        private void OnRemovePhoneNumberExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            _friendReposetory.RemovePhoneNumber(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = _friendReposetory.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddPhoneNumberExecute()
        {
            var newNumber = new FriendPhoneNumberWrapper(new FriendPhoneNumber());
            newNumber.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Friend.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = "";
        }
    }
}