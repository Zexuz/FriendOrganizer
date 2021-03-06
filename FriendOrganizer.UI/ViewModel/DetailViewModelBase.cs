﻿using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Servicies;
using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        protected readonly IEventAggregator EventAggregator;
        protected readonly IMessageDialogService MessageDialogService;
        private bool _hasChanges;
        private int _id;
        private string _title;

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CloseDetailViewCommand { get; }

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

        public string Title
        {
            get { return _title; }
            protected set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public DetailViewModelBase(IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            EventAggregator = eventAggregator;
            MessageDialogService = messageDialogService;
            SaveCommand = new DelegateCommand(() => OnSaveExecute(), OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            CloseDetailViewCommand = new DelegateCommand(OnCloseDetailViewExecute);
        }

        protected virtual void RaseCollectionSavedEvent()
        {
            EventAggregator.GetEvent<AfterCollectionSavedEvent>()
                .Publish(new AfterCollectionSavedEventArgs
                {
                    ViewModelName = this.GetType().Name
                });
        }

        protected virtual async void OnCloseDetailViewExecute()
        {
            if (HasChanges)
            {
                var res = await MessageDialogService.ShowOkCancelDialog("You have made changes, close this item?", "Question");
                if (res == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            EventAggregator.GetEvent<AfterDetailClosedEvent>().Publish(new AfterDetailClosedEventArgs
            {
                Id = this.Id,
                ViewModelName = this.GetType().Name
            });
        }

        public abstract Task LoadAsync(int id);

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
        
        protected async Task SaveWithOptimisticConcurrencyAsync(Func<Task> saveFunc,
            Action afterSaveAction)
        {
            try
            {
                await saveFunc();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var databaseValues = ex.Entries.Single().GetDatabaseValues();
                if (databaseValues == null)
                {
                    await MessageDialogService.ShowInfoDialog("The entity has been deleted by another user");
                    RaiseDetailDeletedEvent(Id);
                    return;
                }

                var result = await MessageDialogService.ShowOkCancelDialog("The entity has been changed in "
                                                                     + "the meantime by someone else. Click OK to save your changes anyway, click Cancel "
                                                                     + "to reload the entity from the database.", "Question");

                if (result == MessageDialogResult.Ok)
                {
                    // Update the original values with database-values
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    await saveFunc();
                }
                else
                {
                    // Reload entity from database
                    await ex.Entries.Single().ReloadAsync();
                    await LoadAsync(Id);
                }
            };

            afterSaveAction();
        }
    }
}