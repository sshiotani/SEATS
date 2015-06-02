namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "UserLocationName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "UserLocationName");
        }
    }
}
