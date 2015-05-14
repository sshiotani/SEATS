namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class counselor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Counselors", "PersonID", c => c.Int());
            AddColumn("dbo.Counselors", "School", c => c.String());
            AlterColumn("dbo.Counselors", "SchoolID", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Counselors", "SchoolID", c => c.Int(nullable: false));
            DropColumn("dbo.Counselors", "School");
            DropColumn("dbo.Counselors", "PersonID");
        }
    }
}
