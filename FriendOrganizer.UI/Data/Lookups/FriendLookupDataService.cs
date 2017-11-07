﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FriendOragnizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Lookups
{
    public class FriendLookupDataService : IFriendLookupDataService, IProgramminLanguageLookupDataService
    {
        private readonly Func<FriendOrganizerDbContext> _contextCreator;

        public FriendLookupDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetFriendLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Friends.AsNoTracking()
                    .Select(f =>
                        new LookupItem
                        {
                            DisplayMember = f.FirstName + " " + f.LastName,
                            Id = f.Id
                        })
                    .ToListAsync();
            }
        }
        
        public async Task<IEnumerable<LookupItem>> GetProgrammingLanguageLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.ProgramminLanguages.AsNoTracking()
                    .Select(f =>
                        new LookupItem
                        {
                            DisplayMember = f.Name,
                            Id = f.Id
                        })
                    .ToListAsync();
            }
        }
    }
}