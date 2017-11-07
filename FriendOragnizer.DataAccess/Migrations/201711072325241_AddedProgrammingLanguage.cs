namespace FriendOragnizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProgrammingLanguage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgramminLanguage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Friend", "FavoriteLanguageId", c => c.Int());
            AddColumn("dbo.Friend", "FavoriteProgrammingLangueage_Id", c => c.Int());
            CreateIndex("dbo.Friend", "FavoriteProgrammingLangueage_Id");
            AddForeignKey("dbo.Friend", "FavoriteProgrammingLangueage_Id", "dbo.ProgramminLanguage", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friend", "FavoriteProgrammingLangueage_Id", "dbo.ProgramminLanguage");
            DropIndex("dbo.Friend", new[] { "FavoriteProgrammingLangueage_Id" });
            DropColumn("dbo.Friend", "FavoriteProgrammingLangueage_Id");
            DropColumn("dbo.Friend", "FavoriteLanguageId");
            DropTable("dbo.ProgramminLanguage");
        }
    }
}
