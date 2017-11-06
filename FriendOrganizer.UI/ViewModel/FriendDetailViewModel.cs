using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel:ViewModelBase, IFriendDetailViewModel
    {
        private readonly IFriendDataService _dataService;
        private Friend _friend;

        public Friend Friend
        {
            get => _friend;
            private set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }
        
        
        public FriendDetailViewModel(IFriendDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task LoadAsync(int friendId)
        {
            Friend = await _dataService.GetByIdAsync(friendId);
        }
    }
}