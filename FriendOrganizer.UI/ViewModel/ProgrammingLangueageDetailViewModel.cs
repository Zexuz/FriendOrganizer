using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class ProgrammingLangueageDetailViewModel : DetailViewModelBase
    {
        private readonly IProgrammingLangueageRepository _programmingLangueageRepository;
        private ProgramminLanguageWrapper _selectedProgrammingLanguage;

        public ObservableCollection<ProgramminLanguageWrapper> ProgrammingLanguages { get; set; }

        public ICommand RemoveCommand { get; set; }
        public ICommand AddCommand { get; set; }

        public ProgramminLanguageWrapper SelectedProgrammingLanguage
        {
            get { return _selectedProgrammingLanguage; }
            set
            {
                _selectedProgrammingLanguage = value;
                OnPropertyChanged();
                ((DelegateCommand) RemoveCommand).RaiseCanExecuteChanged();
            }
        }


        public ProgrammingLangueageDetailViewModel(IEventAggregator eventAggregator,
            IProgrammingLangueageRepository programmingLangueageRepository,
            IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
            _programmingLangueageRepository = programmingLangueageRepository;
            Title = "Programming Langueages";
            ProgrammingLanguages = new ObservableCollection<ProgramminLanguageWrapper>();

            AddCommand = new DelegateCommand(OnAddExecute);
            RemoveCommand = new DelegateCommand(OnRemoveExecute, OnRemoveCanExecute);
        }


        private bool OnRemoveCanExecute()
        {
            return SelectedProgrammingLanguage != null;
        }

        private async void OnRemoveExecute()
        {
            var isReferenced =
                await _programmingLangueageRepository.IsReferencedByFriendAsync(
                    SelectedProgrammingLanguage.Id);
            if (isReferenced)
            {
                MessageDialogService.ShowInfoDialog($"The language {SelectedProgrammingLanguage.Name}" +
                                                    $" can't be removed, as it is referenced by at least one friend");
                return;
            }

            SelectedProgrammingLanguage.PropertyChanged -= Wrapper_PropertyChanged;
            _programmingLangueageRepository.Remove(SelectedProgrammingLanguage.Model);
            ProgrammingLanguages.Remove(SelectedProgrammingLanguage);
            SelectedProgrammingLanguage = null;
            HasChanges = _programmingLangueageRepository.HasChanges();
            ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddExecute()
        {
            var wrapper = new ProgramminLanguageWrapper(new ProgramminLanguage());
            wrapper.PropertyChanged += Wrapper_PropertyChanged;
            _programmingLangueageRepository.Add(wrapper.Model);
            ProgrammingLanguages.Add(wrapper);

            wrapper.Name = "";
        }

        public override async Task LoadAsync(int id)
        {
            Id = id;
            foreach (var wrapper in ProgrammingLanguages)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }

            ProgrammingLanguages.Clear();

            var langueages = await _programmingLangueageRepository.GetAllAsync();
            foreach (var model in langueages)
            {
                var wrapper = new ProgramminLanguageWrapper(model);
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
                ProgrammingLanguages.Add(wrapper);
            }
        }

        private void Wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _programmingLangueageRepository.HasChanges();
            }
            if (e.PropertyName == nameof(ProgramminLanguageWrapper.HasErrors))
            {
                ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            }
        }

        protected override void OnDeleteExecute()
        {
            throw new System.NotImplementedException();
        }

        protected override bool OnSaveCanExecute()
        {
            return HasChanges && ProgrammingLanguages.All(p => !p.HasErrors);
        }

        protected override async void OnSaveExecute()
        {
            try
            {
                await _programmingLangueageRepository.SaveAsync();
                HasChanges = _programmingLangueageRepository.HasChanges();
                RaseCollectionSavedEvent();
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }

                MessageDialogService.ShowInfoDialog("Error while saving entities, the data will be reloaded. Details: " + e.Message);
                await LoadAsync(Id);
            }
        }
    }
}