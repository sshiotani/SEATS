namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student", "ParentID", c => c.Int());
            AddColumn("dbo.Student", "CounselorID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Student", "CounselorID");
            DropColumn("dbo.Student", "ParentID");
        }
    }
}
