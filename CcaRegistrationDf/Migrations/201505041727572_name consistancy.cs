namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nameconsistancy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MainTables", "CategoryID", c => c.Int());
            AddColumn("dbo.MainTables", "CourseID", c => c.Int());
            AddColumn("dbo.MainTables", "CreditID", c => c.Int());
            AddColumn("dbo.MainTables", "Course2ndSemesterID", c => c.Int());
            AddColumn("dbo.MainTables", "SessionID", c => c.Int(nullable: false));
            DropColumn("dbo.MainTables", "CourseCategoryID");
            DropColumn("dbo.MainTables", "CourseNameID");
            DropColumn("dbo.MainTables", "CourseCreditID");
            DropColumn("dbo.MainTables", "CourseName2ndSemesterID");
            DropColumn("dbo.MainTables", "SemesterID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MainTables", "SemesterID", c => c.Int(nullable: false));
            AddColumn("dbo.MainTables", "CourseName2ndSemesterID", c => c.Int());
            AddColumn("dbo.MainTables", "CourseCreditID", c => c.Int());
            AddColumn("dbo.MainTables", "CourseNameID", c => c.Int());
            AddColumn("dbo.MainTables", "CourseCategoryID", c => c.Int());
            DropColumn("dbo.MainTables", "SessionID");
            DropColumn("dbo.MainTables", "Course2ndSemesterID");
            DropColumn("dbo.MainTables", "CreditID");
            DropColumn("dbo.MainTables", "CourseID");
            DropColumn("dbo.MainTables", "CategoryID");
        }
    }
}
