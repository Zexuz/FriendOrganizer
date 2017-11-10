using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac.Features.Indexed;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Servicies;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public INavigationViewModel NavigationViewModel { get; }

        public ObservableCollection<IDetailViewModel> DetailViewModels { get; }

        public IDetailViewModel SelectedDetailViewModel
        {
            get => _selectedDetailViewModel;
            set
            {
                _selectedDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateNewDetailCommand { get; }

        private readonly IIndex<string, IDetailViewModel> _detailViewModelCreator;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private IDetailViewModel _selectedDetailViewModel;

        public MainViewModel(
            INavigationViewModel navigationViewModel,
            IIndex<string, IDetailViewModel> detailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService
        )
        {
            DetailViewModels = new ObservableCollection<IDetailViewModel>();
            _detailViewModelCreator = detailViewModelCreator;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;


            _eventAggregator.GetEvent<OpenDetialViewEvent>().Subscribe(OnOpenDetailView);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
            _eventAggregator.GetEvent<AfterDetailClosedEvent>().Subscribe(AfterDetailClosed);

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetialExecute);

            NavigationViewModel = navigationViewModel;
        }

        private void AfterDetailClosed(AfterDetailClosedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        private async void OnOpenDetailView(OpenDetialViewEventArgs args)
        {
            var detailViewModel = DetailViewModels
                .SingleOrDefault(vm => vm.Id == args.Id
                                       && vm.GetType().Name == args.ViewModelName);

            if (detailViewModel == null)
            {
                detailViewModel = _detailViewModelCreator[args.ViewModelName];
                await detailViewModel.LoadAsync(args.Id);
                DetailViewModels.Add(detailViewModel);
            }

            SelectedDetailViewModel = detailViewModel;
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void RemoveDetailViewModel(int id, string viewModelName)
        {
            var detailViewModel = DetailViewModels
                .SingleOrDefault(vm => vm.Id == id
                                       && vm.GetType().Name == viewModelName);

            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }

        private void OnCreateNewDetialExecute(Type type)
        {
            OnOpenDetailView(new OpenDetialViewEventArgs
            {
                ViewModelName = type.Name
            });
        }
    }
}