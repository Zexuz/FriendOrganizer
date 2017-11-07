using System.Windows.Input;
using FriendOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationItemViewModel:ViewModelBase
    {
        private string _displayMember;
        private IEventAggregator _eventAggregator;

        public int Id { get; }

        public string DisplayMember
        {
            get => _displayMember;
            set
            {
                _displayMember = value; 
                OnPropertyChanged();
            }
        }

        public ICommand OpenFriendDetailViewCommand { get; set; }


        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            Id = id;
            _displayMember = displayMember;
            _eventAggregator = eventAggregator;
            OpenFriendDetailViewCommand = new DelegateCommand(OnOpenFriendDetailView);
        }

        private void OnOpenFriendDetailView()
        {
            _eventAggregator.GetEvent<OpenFriendDetialViewEvent>()
                .Publish(Id);
        }
    }
}