using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        public ObservableCollection<LookupItem> Friends { get; }

        private readonly IFriendLookupDataService _friendLookupService;

        public NavigationViewModel(IFriendLookupDataService friendLookupService)
        {
            _friendLookupService = friendLookupService;
            Friends = new ObservableCollection<LookupItem>();
        }


        public async Task LoadAsync()
        {
            var lookup = await _friendLookupService.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(item);
            }
        }
        
    }
}