namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class counselorfix1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Counselors", "PersonID");
            DropColumn("dbo.Counselors", "SchoolID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Counselors", "SchoolID", c => c.Int());
            AddColumn("dbo.Counselors", "PersonID", c => c.Int(nullable: false));
        }
    }
}
