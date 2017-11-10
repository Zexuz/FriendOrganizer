using System.Threading.Tasks;

namespace FriendOrganizer.UI.View.Servicies
{
    public interface IMessageDialogService
    {
        Task<MessageDialogResult> ShowOkCancelDialog(string text, string title);
        Task ShowInfoDialog(string text);
    }
}