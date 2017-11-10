using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositries
{
    public interface IProgrammingLangueageRepository:IGenericRepository<ProgramminLanguage>
    {
        Task<bool> IsReferencedByFriendAsync(int programmingLangueageId);
        
    }
}