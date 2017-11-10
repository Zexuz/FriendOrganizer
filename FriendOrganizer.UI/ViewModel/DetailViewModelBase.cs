using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.UI.Event;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        protected readonly IEventAggregator EventAggregator;
        private bool _hasChanges;
        private int _id;

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        
        public int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                _hasChanges = value;
                OnPropertyChanged();
                ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            }
        }
        
        public DetailViewModelBase(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }
        public abstract Task LoadAsync(int? id);
        
        protected abstract void OnDeleteExecute();

        protected abstract bool OnSaveCanExecute();

        protected abstract void OnSaveExecute();

        protected virtual void RaiseDetailDeletedEvent(int modelId)
        {
            EventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(
                new AfterDetailDeletedEventArgs
                {
                    Id = modelId,
                    ViewModelName = this.GetType().Name
                });
        }

        protected virtual void RaiseDetailSavedEvent(int modelId, string displayMember)
        {
            EventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(new AfterDetailSavedEventArgs
            {
                Id = modelId,
                DisplayMember = displayMember,
                ViewModelName = this.GetType().Name
            });
        }

    }
}