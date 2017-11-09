using System;
using System.Data.Entity;
using System.Threading.Tasks;
using FriendOragnizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositries
{
    public class FriendReposetory :GenericRepository<Friend,FriendOrganizerDbContext>, IFriendReposetory
    {
        public FriendReposetory(FriendOrganizerDbContext context):base(context)
        {
        } 

        public override async Task<Friend> GetByIdAsync(int friendId)
        {
            return await Context.Friends
                .Include(f => f.PhoneNumbers)
                .SingleAsync(f => f.Id == friendId);
        }

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            Context.FriendPhoneNumbers.Remove(model);
        }
    }
}