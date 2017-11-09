using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Servicies;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public INavigationViewModel NavigationViewModel { get; }

        public IDetailViewModel DetailViewModel
        {
            get => _detailViewModel;
            private set
            {
                _detailViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateNewFriendCommand { get; }

        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
        private IDetailViewModel _detailViewModel;

        public MainViewModel(
            INavigationViewModel navigationViewModel,
            Func<IFriendDetailViewModel> friendDetailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService
        )
        {
            _friendDetailViewModelCreator = friendDetailViewModelCreator;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;


            _eventAggregator.GetEvent<OpenFriendDetialViewEvent>().Subscribe(OnOpenFriendDetailView);
            _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Subscribe(AfterFriendDeleted);

            CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendExecute);

            NavigationViewModel = navigationViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        private async void OnOpenFriendDetailView(int? friendId)
        {
            if (DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var res = _messageDialogService.ShowOkCancelDialog("You have made changes. Navigate away?", "Question");
                if (res == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            DetailViewModel = _friendDetailViewModelCreator();
            await DetailViewModel.LoadAsync(friendId);
        }

        private void AfterFriendDeleted(int friendId)
        {
            DetailViewModel = null;
        }
        
        private void OnCreateNewFriendExecute()
        {
            OnOpenFriendDetailView(null);
        }
    }
}