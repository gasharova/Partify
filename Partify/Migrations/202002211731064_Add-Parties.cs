namespace Partify.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParties : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Parties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerId = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.OwnerId, cascadeDelete: true)
                .Index(t => t.OwnerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Parties", "OwnerId", "dbo.Users");
            DropIndex("dbo.Parties", new[] { "OwnerId" });
            DropTable("dbo.Parties");
        }
    }
}
