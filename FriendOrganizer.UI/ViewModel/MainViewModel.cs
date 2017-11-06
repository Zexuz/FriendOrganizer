using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IFriendDataService _friendDataService;
        private Friend _selectedFriend;

        public ObservableCollection<Friend> Friends { get; set; }

        public Friend SelectedFriend
        {
            get => _selectedFriend;
            set
            {
                _selectedFriend = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(IFriendDataService friendDataService)
        {
            _friendDataService = friendDataService;
            Friends = new ObservableCollection<Friend>();
        }

        public async Task LoadAsync()
        {
            Friends.Clear();
            foreach (var friend in await _friendDataService.GetAllAsync())
            {
                Friends.Add(friend);
            }
        }
    }
}