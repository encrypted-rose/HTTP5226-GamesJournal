namespace GamesJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gameplayers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlayerGames",
                c => new
                    {
                        Player_PlayerId = c.Int(nullable: false),
                        Game_GameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Player_PlayerId, t.Game_GameId })
                .ForeignKey("dbo.Players", t => t.Player_PlayerId, cascadeDelete: true)
                .ForeignKey("dbo.Games", t => t.Game_GameId, cascadeDelete: true)
                .Index(t => t.Player_PlayerId)
                .Index(t => t.Game_GameId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerGames", "Game_GameId", "dbo.Games");
            DropForeignKey("dbo.PlayerGames", "Player_PlayerId", "dbo.Players");
            DropIndex("dbo.PlayerGames", new[] { "Game_GameId" });
            DropIndex("dbo.PlayerGames", new[] { "Player_PlayerId" });
            DropTable("dbo.PlayerGames");
        }
    }
}
