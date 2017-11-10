using System.Collections.Generic;
using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositries
{
    public interface IMeetingRepository:IGenericRepository<Meeting>
    {
        Task<List<Friend>> GetAllFriendsAsync();
    }
}