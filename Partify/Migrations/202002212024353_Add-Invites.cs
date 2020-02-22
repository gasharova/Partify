namespace Partify.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        PartyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parties", t => t.PartyId)
                .ForeignKey("dbo.Users", t => t.ReceiverId)
                .ForeignKey("dbo.Users", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId)
                .Index(t => t.PartyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invites", "SenderId", "dbo.Users");
            DropForeignKey("dbo.Invites", "ReceiverId", "dbo.Users");
            DropForeignKey("dbo.Invites", "PartyId", "dbo.Parties");
            DropIndex("dbo.Invites", new[] { "PartyId" });
            DropIndex("dbo.Invites", new[] { "ReceiverId" });
            DropIndex("dbo.Invites", new[] { "SenderId" });
            DropTable("dbo.Invites");
        }
    }
}
