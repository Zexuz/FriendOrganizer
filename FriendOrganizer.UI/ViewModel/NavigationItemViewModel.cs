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
        private readonly string _detailViewModelName;

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

        public ICommand OpenDetailViewCommand { get; set; }


        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator,string detailViewModelName)
        {
            Id = id;
            _displayMember = displayMember;
            _eventAggregator = eventAggregator;
            _detailViewModelName = detailViewModelName;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
        }

        private void OnOpenDetailViewExecute()
        {
            _eventAggregator.GetEvent<OpenDetialViewEvent>()
                .Publish(new OpenDetialViewEventArgs
                {
                    Id = Id,
                    ViewModelName =  _detailViewModelName
                });;
        }
    }
}