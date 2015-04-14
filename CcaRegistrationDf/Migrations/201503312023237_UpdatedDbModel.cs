namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDbModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "OnlineProviderID", c => c.Int(nullable: false));
            CreateIndex("dbo.Courses", "OnlineProviderID");
            AddForeignKey("dbo.Courses", "OnlineProviderID", "dbo.OnlineProviders", "ID", cascadeDelete: true);
            DropColumn("dbo.Courses", "providerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "providerID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Courses", "OnlineProviderID", "dbo.OnlineProviders");
            DropIndex("dbo.Courses", new[] { "OnlineProviderID" });
            DropColumn("dbo.Courses", "OnlineProviderID");
        }
    }
}
