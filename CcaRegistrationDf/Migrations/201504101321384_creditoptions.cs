namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creditoptions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "CreditOptions", c => c.String());
            DropColumn("dbo.EnrollmentLocations", "DistrictId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EnrollmentLocations", "DistrictId", c => c.String());
            DropColumn("dbo.Courses", "CreditOptions");
        }
    }
}
