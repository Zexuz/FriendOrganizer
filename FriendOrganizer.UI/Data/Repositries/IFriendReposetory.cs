using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositries
{
    public interface IFriendReposetory
    {
        Task<Friend> GetByIdAsync(int friendId);
        Task SaveAsync();
        bool HasChanges();
        void Add(Friend friend);
        void Remove(Friend friendModel);
        void RemovePhoneNumber(FriendPhoneNumber model);
    }
}