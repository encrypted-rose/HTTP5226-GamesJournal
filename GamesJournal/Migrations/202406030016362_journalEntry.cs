namespace GamesJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class journalEntry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Journals",
                c => new
                    {
                        JournalEntryId = c.Int(nullable: false, identity: true),
                        JournalEntryTitle = c.String(),
                        JournalEntry = c.String(),
                        EntryDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JournalEntryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Journals");
        }
    }
}
