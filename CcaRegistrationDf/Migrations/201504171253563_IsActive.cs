namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Courses", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.OnlineProviders", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OnlineProviders", "IsActive");
            DropColumn("dbo.Courses", "IsActive");
            DropColumn("dbo.Categories", "IsActive");
        }
    }
}
