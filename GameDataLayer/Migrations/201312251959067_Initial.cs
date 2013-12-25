namespace GameDataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        GameId = c.Int(nullable: false, identity: true),
                        Level = c.Int(nullable: false),
                        OnHold = c.Boolean(nullable: false),
                        Tournament_TournamentId = c.Int(),
                        Winner_PlayerId = c.Int(),
                    })
                .PrimaryKey(t => t.GameId)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_TournamentId)
                .ForeignKey("dbo.Players", t => t.Winner_PlayerId)
                .Index(t => t.Tournament_TournamentId)
                .Index(t => t.Winner_PlayerId);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        PlayerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Tournament_TournamentId = c.Int(),
                    })
                .PrimaryKey(t => t.PlayerId)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_TournamentId)
                .Index(t => t.Tournament_TournamentId);
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        TournamentId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        Winner_PlayerId = c.Int(),
                    })
                .PrimaryKey(t => t.TournamentId)
                .ForeignKey("dbo.Players", t => t.Winner_PlayerId)
                .Index(t => t.Winner_PlayerId);
            
            CreateTable(
                "dbo.GamePlayers",
                c => new
                    {
                        Game_GameId = c.Int(nullable: false),
                        Player_PlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Game_GameId, t.Player_PlayerId })
                .ForeignKey("dbo.Games", t => t.Game_GameId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.Player_PlayerId, cascadeDelete: true)
                .Index(t => t.Game_GameId)
                .Index(t => t.Player_PlayerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "Winner_PlayerId", "dbo.Players");
            DropForeignKey("dbo.Games", "Tournament_TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.GamePlayers", "Player_PlayerId", "dbo.Players");
            DropForeignKey("dbo.GamePlayers", "Game_GameId", "dbo.Games");
            DropForeignKey("dbo.Players", "Tournament_TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.Tournaments", "Winner_PlayerId", "dbo.Players");
            DropIndex("dbo.Games", new[] { "Winner_PlayerId" });
            DropIndex("dbo.Games", new[] { "Tournament_TournamentId" });
            DropIndex("dbo.GamePlayers", new[] { "Player_PlayerId" });
            DropIndex("dbo.GamePlayers", new[] { "Game_GameId" });
            DropIndex("dbo.Players", new[] { "Tournament_TournamentId" });
            DropIndex("dbo.Tournaments", new[] { "Winner_PlayerId" });
            DropTable("dbo.GamePlayers");
            DropTable("dbo.Tournaments");
            DropTable("dbo.Players");
            DropTable("dbo.Games");
        }
    }
}
