namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Notes");
        }
    }
}
