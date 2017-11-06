using FriendOrganizer.Model;

namespace FriendOragnizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

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
        }
    }
}