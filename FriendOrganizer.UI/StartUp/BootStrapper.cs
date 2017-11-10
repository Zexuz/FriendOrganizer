using Autofac;
using FriendOragnizer.DataAccess;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositries;
using FriendOrganizer.UI.View.Servicies;
using FriendOrganizer.UI.ViewModel;
using Prism.Events;
using IContainer = Autofac.IContainer;

namespace FriendOrganizer.UI.StartUp
{
    public class Bootstrapper
    {
        public IContainer Bootstapp()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<FriendOrganizerDbContext>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FriendDetailViewModel>().Keyed<IDetailViewModel>(nameof(FriendDetailViewModel));
            builder.RegisterType<MeetingDetailViewModel>().Keyed<IDetailViewModel>(nameof(MeetingDetailViewModel));

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<FriendReposetory>().As<IFriendReposetory>();
            builder.RegisterType<MeetingRepository>().As<IMeetingRepository>();

            return builder.Build();
        }
    }
}