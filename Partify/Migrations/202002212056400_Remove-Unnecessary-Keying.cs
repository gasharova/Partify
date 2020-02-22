namespace Partify.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUnnecessaryKeying : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Parties", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Party_Id", "dbo.Parties");
            DropIndex("dbo.Parties", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "Party_Id" });
            DropColumn("dbo.Parties", "User_Id");
            DropColumn("dbo.Users", "Party_Id");
        }
        
        public override void Down()
        {
        }
    }
}
