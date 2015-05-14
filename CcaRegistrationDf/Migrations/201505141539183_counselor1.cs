namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class counselor1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Counselors", "CounselorEmail", c => c.String());
            AlterColumn("dbo.Counselors", "CounselorFirstName", c => c.String());
            AlterColumn("dbo.Counselors", "CounselorLastName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Counselors", "CounselorLastName", c => c.String(nullable: false));
            AlterColumn("dbo.Counselors", "CounselorFirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Counselors", "CounselorEmail", c => c.String(nullable: false));
        }
    }
}
