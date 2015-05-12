namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addControllers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Counselors", "SchoolID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Counselors", "SchoolID");
        }
    }
}
