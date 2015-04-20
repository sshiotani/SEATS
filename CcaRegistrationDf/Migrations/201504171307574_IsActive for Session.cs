namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsActiveforSession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sessions", "IsActive");
        }
    }
}
