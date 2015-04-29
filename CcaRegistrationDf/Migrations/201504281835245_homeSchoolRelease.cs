namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class homeSchoolRelease : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MainTables", "HasHomeSchoolRelease", c => c.Boolean(nullable: false));
            DropColumn("dbo.MainTables", "IsStudentSigned");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MainTables", "IsStudentSigned", c => c.Boolean(nullable: false));
            DropColumn("dbo.MainTables", "HasHomeSchoolRelease");
        }
    }
}
