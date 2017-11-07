using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using FriendOrganizer.Model;

namespace FriendOragnizer.DataAccess
{
    public class FriendOrganizerDbContext:DbContext
    {

        public FriendOrganizerDbContext():base("FriendOrganizerDb")
        {
            
        }
        public DbSet<Friend> Friends { get; set; }

        public DbSet<ProgramminLanguage> ProgramminLanguages{ get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}