using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositries;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private readonly IFriendReposetory _friendReposetory;
        private readonly IEventAggregator _eventAggregator;
        private FriendWrapper _friend;
        private bool _hasChanges;

        public ICommand SaveCommand { get; }

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


        public FriendDetailViewModel(IFriendReposetory friendReposetory, IEventAggregator eventAggregator)
        {
            _friendReposetory = friendReposetory;
            _eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public async Task LoadAsync(int? friendId)
        {
            var friend = friendId.HasValue
                ?await _friendReposetory.GetByIdAsync(friendId.Value)
                :CreateNewFriend();
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
            if(Friend.Id == 0)
            {
                Friend.FirstName = "";
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
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(
                new AfterFriendSavedEventArgs
                {
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}",
                    Id = Friend.Id
                }
            );
        }

        private bool OnSaveCanExecute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges;
        }
    }
}