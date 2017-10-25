using System.Collections.ObjectModel;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewMOdel:ViewModelBase
    {
        private readonly IFriendDataService _friendDataService;
        private Friend _selecetedFriend;

        public ObservableCollection<Friend> Friends { get; set; }

        public Friend SelecetedFriend
        {
            get => _selecetedFriend;
            set
            {
                OnPropertyChanged();
                _selecetedFriend = value;
            }
        }

        public MainViewMOdel(IFriendDataService friendDataService)
        {
            _friendDataService = friendDataService;
            Friends = new ObservableCollection<Friend>();
        }

        public void Load()
        {
            Friends.Clear();
            foreach (var friend in _friendDataService.GetAll())
            {
                Friends.Add(friend);
            }
        }


     
    }
}