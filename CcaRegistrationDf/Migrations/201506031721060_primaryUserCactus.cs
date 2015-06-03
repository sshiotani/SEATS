namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class primaryUserCactus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrimaryUsers", "EnrollmentLocationID", c => c.Int(nullable: false));
            AddColumn("dbo.PrimaryUsers", "EnrollmentLocationSchoolNameID", c => c.Int(nullable: false));
            DropColumn("dbo.PrimaryUsers", "CactusInstitutionID");
            DropColumn("dbo.PrimaryUsers", "CactusSchoolID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PrimaryUsers", "CactusSchoolID", c => c.Int(nullable: false));
            AddColumn("dbo.PrimaryUsers", "CactusInstitutionID", c => c.Int(nullable: false));
            DropColumn("dbo.PrimaryUsers", "EnrollmentLocationSchoolNameID");
            DropColumn("dbo.PrimaryUsers", "EnrollmentLocationID");
        }
    }
}
