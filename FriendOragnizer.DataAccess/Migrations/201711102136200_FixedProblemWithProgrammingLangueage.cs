namespace FriendOragnizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedProblemWithProgrammingLangueage : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Friend", name: "FavoriteProgrammingLangueage_Id", newName: "FavoriteLangueage_Id");
            RenameIndex(table: "dbo.Friend", name: "IX_FavoriteProgrammingLangueage_Id", newName: "IX_FavoriteLangueage_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Friend", name: "IX_FavoriteLangueage_Id", newName: "IX_FavoriteProgrammingLangueage_Id");
            RenameColumn(table: "dbo.Friend", name: "FavoriteLangueage_Id", newName: "FavoriteProgrammingLangueage_Id");
        }
    }
}
