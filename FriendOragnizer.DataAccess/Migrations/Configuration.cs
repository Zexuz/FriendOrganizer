using System;
using System.Collections.Generic;
using System.Linq;
using FriendOrganizer.Model;

namespace FriendOragnizer.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendOragnizer.DataAccess.FriendOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendOragnizer.DataAccess.FriendOrganizerDbContext context)
        {
            context.Friends.AddOrUpdate(
                f => f.FirstName,
                new Friend {FirstName = "Kalle", LastName = "Anka"},
                new Friend {FirstName = "Alexander", LastName = "Anka"},
                new Friend {FirstName = "Knatte", LastName = "Anka"},
                new Friend {FirstName = "Kajsa", LastName = "Anka"}
            );

            context.ProgramminLanguages.AddOrUpdate(
                pl => pl.Name,
                new ProgramminLanguage {Name = "C#"},
                new ProgramminLanguage {Name = "Dart"},
                new ProgramminLanguage {Name = "Kotlin"},
                new ProgramminLanguage {Name = "Js"}
            );

            context.SaveChanges();

            context.FriendPhoneNumbers.AddOrUpdate(a => a.Number,
                new FriendPhoneNumber
                {
                    Number = "+46722050271",
                    FriendId = context.Friends.First().Id
                });

            var friendsList = context.Friends.ToList();
            context.Meetings.AddOrUpdate(m => m.Title,
                new Meeting
                {
                    Title = "Hangout",
                    DateFrom = new DateTime(2018, 11, 10),
                    DateTo = new DateTime(2018, 11, 11),
                    Friends = new List<Friend>
                    {
                        friendsList[0],
                        friendsList[1]
                    }
                });
        }
    }
}