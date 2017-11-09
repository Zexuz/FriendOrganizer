using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositries
{
    public interface IFriendReposetory:IGenericRepository<Friend>
    {
        void RemovePhoneNumber(FriendPhoneNumber model);
    }
}