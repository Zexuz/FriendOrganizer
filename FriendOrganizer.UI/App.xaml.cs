using System.Windows;
using System.Windows.Threading;
using Autofac;
using FriendOrganizer.UI.StartUp;

namespace FriendOrganizer.UI
{
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            
            var container = bootstrapper.Bootstapp();
            
            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {

            e.Handled = true;
        }
    }
}