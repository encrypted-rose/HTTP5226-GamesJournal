namespace GamesJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class journalplayer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journals", "PlayerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Journals", "PlayerId");
            AddForeignKey("dbo.Journals", "PlayerId", "dbo.Players", "PlayerId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Journals", "PlayerId", "dbo.Players");
            DropIndex("dbo.Journals", new[] { "PlayerId" });
            DropColumn("dbo.Journals", "PlayerId");
        }
    }
}
