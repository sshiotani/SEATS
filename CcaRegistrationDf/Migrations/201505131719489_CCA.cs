namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CCA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CCAs", "TeacherCactusID", c => c.Int());
            AddColumn("dbo.CCAs", "TeacherFirstName", c => c.String());
            AddColumn("dbo.CCAs", "TeacherLastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CCAs", "TeacherLastName");
            DropColumn("dbo.CCAs", "TeacherFirstName");
            DropColumn("dbo.CCAs", "TeacherCactusID");
        }
    }
}
