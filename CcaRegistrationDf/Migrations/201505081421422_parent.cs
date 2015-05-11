namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class parent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OnlineCourse", "ClassSession_ID", "dbo.ClassSession");
            DropIndex("dbo.OnlineCourse", new[] { "ClassSession_ID" });
            AddColumn("dbo.Student", "EnrollmentLocationSchoolNamesID", c => c.Int());
            AlterColumn("dbo.Counselor", "CounselorEmail", c => c.String(nullable: false));
            AlterColumn("dbo.Counselor", "CounselorFirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Counselor", "CounselorLastName", c => c.String(nullable: false));
            AlterColumn("dbo.Parent", "GuardianEmail", c => c.String(nullable: false));
            AlterColumn("dbo.Parent", "GuardianFirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Parent", "GuardianLastName", c => c.String(nullable: false));
            DropColumn("dbo.OnlineCourse", "ClassSession_ID");
            DropColumn("dbo.Student", "CounselorID");
            DropTable("dbo.ClassSession");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ClassSession",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Student", "CounselorID", c => c.Int());
            AddColumn("dbo.OnlineCourse", "ClassSession_ID", c => c.Int());
            AlterColumn("dbo.Parent", "GuardianLastName", c => c.String());
            AlterColumn("dbo.Parent", "GuardianFirstName", c => c.String());
            AlterColumn("dbo.Parent", "GuardianEmail", c => c.String());
            AlterColumn("dbo.Counselor", "CounselorLastName", c => c.String());
            AlterColumn("dbo.Counselor", "CounselorFirstName", c => c.String());
            AlterColumn("dbo.Counselor", "CounselorEmail", c => c.String());
            DropColumn("dbo.Student", "EnrollmentLocationSchoolNamesID");
            CreateIndex("dbo.OnlineCourse", "ClassSession_ID");
            AddForeignKey("dbo.OnlineCourse", "ClassSession_ID", "dbo.ClassSession", "ID");
        }
    }
}
