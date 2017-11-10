using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FriendOrganizer.UI.Data.Repositries;
using FriendOrganizer.UI.View.Servicies;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class ProgramminLangueageDetailViewModel:DetailViewModelBase
    {
        private readonly IProgrammingLangueageRepository _programmingLangueageRepository;

        public ObservableCollection<ProgramminLanguageWrapper> ProgrammingLanguages{ get; set; }
        
        public ProgramminLangueageDetailViewModel(IEventAggregator eventAggregator,
            IProgrammingLangueageRepository programmingLangueageRepository,
            IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
            _programmingLangueageRepository = programmingLangueageRepository;
            Title = "Programming Langueages";
            ProgrammingLanguages = new ObservableCollection<ProgramminLanguageWrapper>();
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
            if(!HasChanges)
            {
                HasChanges = _programmingLangueageRepository.HasChanges();
            }
            if(e.PropertyName == nameof(ProgramminLanguageWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
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
            await _programmingLangueageRepository.SaveAsync();
            HasChanges = _programmingLangueageRepository.HasChanges();
        }
    }
}