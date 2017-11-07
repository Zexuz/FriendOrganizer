using System.Windows;

namespace FriendOrganizer.UI.View.Servicies
{
    public class MessageDialogService : IMessageDialogService
    {
        
        public MessageDialogResult ShowOkCancelDialog(string text,string title)
        {
            var res = MessageBox.Show(text,title, MessageBoxButton.OKCancel);
            return res == MessageBoxResult.Cancel ? MessageDialogResult.Cancel : MessageDialogResult.Ok;
        }
        
    }

    public enum MessageDialogResult
    {
        Ok,
        Cancel
    }
}