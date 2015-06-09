namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class counselor1 : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.Counselors", "Email", c => c.String());
            //AlterColumn("dbo.Counselors", "FirstName", c => c.String());
            //AlterColumn("dbo.Counselors", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.Counselors", "LastName", c => c.String(nullable: false));
            //AlterColumn("dbo.Counselors", "FirstName", c => c.String(nullable: false));
            //AlterColumn("dbo.Counselors", "Email", c => c.String(nullable: false));
        }
    }
}
