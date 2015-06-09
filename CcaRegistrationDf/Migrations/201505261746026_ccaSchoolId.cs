namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ccaSchoolId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CCAs", "EnrollmentLocationSchoolNamesID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CCAs", "EnrollmentLocationSchoolNamesID");
        }
    }
}
