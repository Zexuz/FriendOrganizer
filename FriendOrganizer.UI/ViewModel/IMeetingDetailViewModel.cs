using System;
using System.Threading.Tasks;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositries;
using FriendOrganizer.UI.View.Servicies;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public interface IMeetingDetailViewModel:IDetailViewModel
    {
        
    }

    public class MeetingDetailViewModel :DetailViewModelBase, IMeetingDetailViewModel
    {

        private readonly IMeetingRepository _meetingRepository;
        private MeetingWrapper _meeting;
        private IMessageDialogService _messageDialogService;

        public MeetingWrapper Meeting
        {
            get { return _meeting; }
            private set
            {
                _meeting = value;
                OnPropertyChanged();
            }
        }

        public MeetingDetailViewModel(IEventAggregator eventAggregator,IMeetingRepository meetingRepository, IMessageDialogService messageDialogService):base(eventAggregator)
        {
            _meetingRepository = meetingRepository;
            _messageDialogService = messageDialogService;
        }

        public override async Task LoadAsync(int? id)
        {
            var meeting = id.HasValue ? await _meetingRepository.GetByIdAsync(id.Value) : CreateNewMeeting();

            InitializeMeeting(meeting);
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (sender, e) =>
            {
                
                if(!HasChanges)
                {
                    HasChanges = _meetingRepository.HasChanges();
                }
                
                if(e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                
            };
        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now,
            };
            _meetingRepository.Add(meeting);
            return meeting;
        }

        protected override void OnDeleteExecute()
        {
            var result =
                _messageDialogService.ShowOkCancelDialog($"Do you really want to delete the meeting {Meeting.Title}",
                    "Question");
            if(result == MessageDialogResult.Ok)
            {
                _meetingRepository.Remove(Meeting.Model);
                _meetingRepository.SaveAsync();
                RaiseDetailDeletedEvent(Meeting.Id);
            }
            
            
        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();
            RaiseDetailSavedEvent(Meeting.Id,Meeting.Title);
        }
    }
}