using System.ComponentModel;
using Autofac;
using FriendOragnizer.DataAccess;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.ViewModel;
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

            builder.RegisterType<FriendOrganizerDbContext>().AsSelf();

            builder.RegisterType<FriendDataService>().As<IFriendDataService>();

            return builder.Build();
        }
    }
}