namespace GamesJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class journalgame : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journals", "GameiId", c => c.Int(nullable: false));
            CreateIndex("dbo.Journals", "GameiId");
            AddForeignKey("dbo.Journals", "GameiId", "dbo.Games", "GameId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Journals", "GameiId", "dbo.Games");
            DropIndex("dbo.Journals", new[] { "GameiId" });
            DropColumn("dbo.Journals", "GameiId");
        }
    }
}
