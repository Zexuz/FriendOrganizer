using Autofac;
using FriendOragnizer.DataAccess;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositries;
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

            builder.RegisterType<FriendOrganizerDbContext>().AsSelf();
            builder.RegisterType<FriendLookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FriendDetailViewModel>().As<IFriendDetailViewModel>();

            builder.RegisterType<FriendReposetory>().As<IFriendReposetory>();

            return builder.Build();
        }
    }
}