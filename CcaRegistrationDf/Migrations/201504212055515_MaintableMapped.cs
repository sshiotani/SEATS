namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaintableMapped : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MainTables", "HasExcessiveFED", c => c.Boolean(nullable: false));
            AddColumn("dbo.MainTables", "CategoryID", c => c.Int());
            AddColumn("dbo.MainTables", "CourseID", c => c.Int());
            AddColumn("dbo.MainTables", "Course2ndSemesterID", c => c.Int());
            AddColumn("dbo.MainTables", "SessionID", c => c.Int(nullable: false));
            AlterColumn("dbo.MainTables", "Grand_Total", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.MainTables", "CourseID");
            DropColumn("dbo.MainTables", "CourseRecord2ndSemesterID");
            DropColumn("dbo.MainTables", "CourseRecordID");
            DropColumn("dbo.MainTables", "IsQ1");
            DropColumn("dbo.MainTables", "IsQ2");
            DropColumn("dbo.MainTables", "IsQ3");
            DropColumn("dbo.MainTables", "IsQ4");
            DropColumn("dbo.MainTables", "ParentTelephone2");
            DropColumn("dbo.MainTables", "SubmissionDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MainTables", "SubmissionDate", c => c.DateTime());
            AddColumn("dbo.MainTables", "ParentTelephone2", c => c.String());
            AddColumn("dbo.MainTables", "IsQ4", c => c.Boolean(nullable: false));
            AddColumn("dbo.MainTables", "IsQ3", c => c.Boolean(nullable: false));
            AddColumn("dbo.MainTables", "IsQ2", c => c.Boolean(nullable: false));
            AddColumn("dbo.MainTables", "IsQ1", c => c.Boolean(nullable: false));
            AddColumn("dbo.MainTables", "CourseRecordID", c => c.String());
            AddColumn("dbo.MainTables", "CourseRecord2ndSemesterID", c => c.String());
            AddColumn("dbo.MainTables", "CourseID", c => c.Int());
            AlterColumn("dbo.MainTables", "Grand_Total", c => c.String());
            DropColumn("dbo.MainTables", "SessionID");
            DropColumn("dbo.MainTables", "Course2ndSemesterID");
            DropColumn("dbo.MainTables", "CourseID");
            DropColumn("dbo.MainTables", "CategoryID");
            DropColumn("dbo.MainTables", "HasExcessiveFED");
        }
    }
}
