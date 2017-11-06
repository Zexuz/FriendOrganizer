namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationItemViewModel:ViewModelBase
    {
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

        private string _displayMember;

        public NavigationItemViewModel(int id, string displayMember)
        {
            Id = id;
            _displayMember = displayMember;
        }
        
    }
}