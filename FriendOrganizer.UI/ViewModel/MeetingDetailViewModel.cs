using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositries;
using FriendOrganizer.UI.View.Servicies;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private readonly IMeetingRepository _meetingRepository;
        private MeetingWrapper _meeting;
        private IMessageDialogService _messageDialogService;
        private Friend _selectedAvalibleFriend;
        private Friend _selectedAddedFriend;
        private List<Friend> _allFriends;

        public ObservableCollection<Friend> AddedFriends { get; set; }
        public ObservableCollection<Friend> AvalibleFriends { get; set; }

        public ICommand AddFriendCommand { get; set; }
        public ICommand RemoveFriendCommand { get; set; }


        public Friend SelectedAddedFriend
        {
            get { return _selectedAddedFriend; }
            set
            {
                _selectedAddedFriend = value;
                OnPropertyChanged();
                ((DelegateCommand) RemoveFriendCommand).RaiseCanExecuteChanged();
            }
        }

        public Friend SelectedAvalibleFriend
        {
            get { return _selectedAvalibleFriend; }
            set
            {
                _selectedAvalibleFriend = value;
                OnPropertyChanged();
                ((DelegateCommand) AddFriendCommand).RaiseCanExecuteChanged();
            }
        }

        public MeetingWrapper Meeting
        {
            get { return _meeting; }
            private set
            {
                _meeting = value;
                OnPropertyChanged();
            }
        }

        public MeetingDetailViewModel(IEventAggregator eventAggregator, IMeetingRepository meetingRepository, IMessageDialogService messageDialogService) :
            base(eventAggregator)
        {
            _meetingRepository = meetingRepository;
            _messageDialogService = messageDialogService;

            AddedFriends = new ObservableCollection<Friend>();
            AvalibleFriends = new ObservableCollection<Friend>();
            AddFriendCommand = new DelegateCommand(OnAddFriendExecure, OnAddFriendCanExecure);
            RemoveFriendCommand = new DelegateCommand(OnRemoveFriendExecure, OnRemoveFriendCanExecure);
        }

        private void OnRemoveFriendExecure()
        {
            var friendToRemove = SelectedAddedFriend;
            
            Meeting.Model.Friends.Remove(friendToRemove);
            AddedFriends.Remove(friendToRemove);
            AvalibleFriends.Add(friendToRemove);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

        }

        private bool OnRemoveFriendCanExecure()
        {
            return SelectedAddedFriend != null;
        }

        private bool OnAddFriendCanExecure()
        {
            return SelectedAvalibleFriend != null;
        }

        private void OnAddFriendExecure()
        {
            var friendToAdd = SelectedAvalibleFriend;
            
            Meeting.Model.Friends.Add(friendToAdd);
            AddedFriends.Add(friendToAdd);
            AvalibleFriends.Remove(friendToAdd);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }


        public override async Task LoadAsync(int? id)
        {
            var meeting = id.HasValue ? await _meetingRepository.GetByIdAsync(id.Value) : CreateNewMeeting();

            Id = meeting.Id;
            
            InitializeMeeting(meeting);

            _allFriends = await _meetingRepository.GetAllFriendsAsync();

            SetupPicklist();
        }

        private void SetupPicklist()
        {
            var meetingFriendIds = Meeting.Model.Friends.Select(f => f.Id).ToList();
            var addedFriends = _allFriends.Where(f => meetingFriendIds.Contains(f.Id)).OrderBy(f => f.FirstName).ToList();
            var avalibleFriends = _allFriends.Except(addedFriends).OrderBy(f => f.FirstName);

            AddedFriends.Clear();
            AvalibleFriends.Clear();
            foreach (var addedFriend in addedFriends)
            {
                AddedFriends.Add(addedFriend);
            }
            foreach (var avalibleFriend in avalibleFriends)
            {
                AvalibleFriends.Add(avalibleFriend);
            }
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (sender, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _meetingRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            if (Meeting.Id == 0)
            {
                Meeting.Title = "";
            }
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
            if (result == MessageDialogResult.Ok)
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
            Id = Meeting.Id;
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }
    }
}