namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MainTables", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.MainTables", "UserId");
            AddForeignKey("dbo.MainTables", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MainTables", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.MainTables", new[] { "UserId" });
            DropColumn("dbo.MainTables", "UserId");
        }
    }
}
