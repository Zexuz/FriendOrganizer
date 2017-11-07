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
                new ProgramminLanguage{Name = "C#"},
                new ProgramminLanguage{Name = "Dart"},
                new ProgramminLanguage{Name = "Kotlin"},
                new ProgramminLanguage{Name = "Js"}
                );
        }
    }
}