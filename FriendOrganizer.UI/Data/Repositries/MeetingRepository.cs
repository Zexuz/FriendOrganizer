﻿using System.Data.Entity;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FriendOragnizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositries
{
    public class MeetingRepository:GenericRepository<Meeting,FriendOrganizerDbContext>, IMeetingRepository
    {
        protected MeetingRepository(FriendOrganizerDbContext context) : base(context)
        {
        }

        public override async Task<Meeting> GetByIdAsync(int id)
        {
            return await Context.Meetings
                .Include(m => m.Friends)
                .SingleAsync(m => m.Id == id);
        }
    }
}