using FriendOragnizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositries
{
    public class ProgrammingLangueageRepository : GenericRepository<ProgramminLanguage,FriendOrganizerDbContext>,IProgrammingLangueageRepository
    {
        public ProgrammingLangueageRepository(FriendOrganizerDbContext context) : base(context)
        {
        }
    }
}