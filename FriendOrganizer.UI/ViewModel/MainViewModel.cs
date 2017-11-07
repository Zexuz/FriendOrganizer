using System;
using System.Threading.Tasks;
using FriendOrganizer.UI.Event;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public INavigationViewModel NavigationViewModel { get; }

        public IFriendDetailViewModel FriendDetailViewModel
        {
            get => _friendDetailViewModel;
            private set
            {
                _friendDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        private readonly IEventAggregator _eventAggregator;
        private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
        private IFriendDetailViewModel _friendDetailViewModel;

        public MainViewModel(INavigationViewModel navigationViewModel, Func<IFriendDetailViewModel > friendDetailViewModelCreator, IEventAggregator eventAggregator)
        {
            _friendDetailViewModelCreator = friendDetailViewModelCreator;
            _eventAggregator = eventAggregator;

            
            _eventAggregator.GetEvent<OpenFriendDetialViewEvent>().Subscribe(OpOpenFriendDetailView);
            
            NavigationViewModel = navigationViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }
        
        private async void OpOpenFriendDetailView(int friendId)
        {
            FriendDetailViewModel = _friendDetailViewModelCreator();
            await FriendDetailViewModel.LoadAsync(friendId);
        }
    }
}