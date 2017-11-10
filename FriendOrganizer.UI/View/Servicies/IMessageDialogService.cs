namespace FriendOrganizer.UI.View.Servicies
{
    public interface IMessageDialogService
    {
        MessageDialogResult ShowOkCancelDialog(string text,string title);
        void ShowInfoDialog(string text);
    }
}