namespace GameDataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeStartDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tournaments", "StartDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tournaments", "StartDate", c => c.DateTime(nullable: false));
        }
    }
}
