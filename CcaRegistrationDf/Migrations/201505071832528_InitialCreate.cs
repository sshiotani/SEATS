namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
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
            
            CreateTable(
                "dbo.OnlineCourse",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryID = c.Int(nullable: false),
                        Name = c.String(),
                        Credit = c.String(),
                        Code = c.String(),
                        OnlineProviderID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        SessionID = c.Int(nullable: false),
                        Notes = c.String(),
                        ClassSession_ID = c.Int(),
                        CourseCategory_ID = c.Int(),
                        Provider_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.OnlineProvider", t => t.OnlineProviderID, cascadeDelete: true)
                .ForeignKey("dbo.Session", t => t.SessionID, cascadeDelete: true)
                .ForeignKey("dbo.ClassSession", t => t.ClassSession_ID)
                .ForeignKey("dbo.CourseCategory", t => t.CourseCategory_ID)
                .ForeignKey("dbo.Provider", t => t.Provider_ID)
                .Index(t => t.CategoryID)
                .Index(t => t.OnlineProviderID)
                .Index(t => t.SessionID)
                .Index(t => t.ClassSession_ID)
                .Index(t => t.CourseCategory_ID)
                .Index(t => t.Provider_ID);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryID = c.Int(nullable: false),
                        Name = c.String(),
                        Credit = c.String(),
                        Code = c.String(),
                        OnlineProviderID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        SessionID = c.Int(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.OnlineProvider", t => t.OnlineProviderID, cascadeDelete: true)
                .ForeignKey("dbo.Session", t => t.SessionID, cascadeDelete: true)
                .Index(t => t.CategoryID)
                .Index(t => t.OnlineProviderID)
                .Index(t => t.SessionID);
            
            CreateTable(
                "dbo.OnlineProvider",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DistrictID = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Session",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Counselor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CounselorCactusID = c.Int(),
                        CounselorEmail = c.String(),
                        CounselorFirstName = c.String(),
                        CounselorLastName = c.String(),
                        CounselorPhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CourseCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CourseCredit",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Parent",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GuardianEmail = c.String(),
                        GuardianFirstName = c.String(),
                        GuardianLastName = c.String(),
                        GuardianPhone1 = c.String(),
                        GuardianPhone2 = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Primary",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Business_Administrator_Email = c.String(),
                        BusinessAdministratorFirstName = c.String(),
                        BusinessAdministratorLastName = c.String(),
                        BusinessAdministratorSignature = c.Boolean(nullable: false),
                        BusinessAdministratorTelephone = c.String(),
                        DateBusinessAdministratorSignature = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Provider",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DistrictID = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        ProviderEmail = c.String(),
                        ProviderFax = c.String(),
                        ProviderFirstName = c.String(),
                        ProviderLastName = c.String(),
                        ProviderPhoneNumber = c.String(),
                        TeacherFirstName = c.String(),
                        TeacherLastName = c.String(),
                        TeacherCactusID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        StudentFirstName = c.String(),
                        StudentLastName = c.String(),
                        SSID = c.Int(),
                        StudentDOB = c.DateTime(),
                        StudentEmail = c.String(),
                        EnrollmentLocationID = c.Int(),
                        GraduationDate = c.DateTime(),
                        HasExcessiveFED = c.Boolean(nullable: false),
                        ExcessiveFEDExplanation = c.String(),
                        ExcessiveFEDReasonCode = c.Int(),
                        IsEarlyGraduate = c.Boolean(nullable: false),
                        IsFeeWaived = c.Boolean(nullable: false),
                        IsIEP = c.Boolean(nullable: false),
                        IsPrimaryEnrollmentVerified = c.Boolean(nullable: false),
                        IsSection504 = c.Boolean(nullable: false),
                        HasHomeSchoolRelease = c.Boolean(nullable: false),
                        SchoolOfRecord = c.String(),
                        StudentBudgetID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OnlineCourse", "Provider_ID", "dbo.Provider");
            DropForeignKey("dbo.OnlineCourse", "CourseCategory_ID", "dbo.CourseCategory");
            DropForeignKey("dbo.OnlineCourse", "ClassSession_ID", "dbo.ClassSession");
            DropForeignKey("dbo.OnlineCourse", "SessionID", "dbo.Session");
            DropForeignKey("dbo.OnlineCourse", "OnlineProviderID", "dbo.OnlineProvider");
            DropForeignKey("dbo.OnlineCourse", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.Course", "SessionID", "dbo.Session");
            DropForeignKey("dbo.Course", "OnlineProviderID", "dbo.OnlineProvider");
            DropForeignKey("dbo.Course", "CategoryID", "dbo.Category");
            DropIndex("dbo.Course", new[] { "SessionID" });
            DropIndex("dbo.Course", new[] { "OnlineProviderID" });
            DropIndex("dbo.Course", new[] { "CategoryID" });
            DropIndex("dbo.OnlineCourse", new[] { "Provider_ID" });
            DropIndex("dbo.OnlineCourse", new[] { "CourseCategory_ID" });
            DropIndex("dbo.OnlineCourse", new[] { "ClassSession_ID" });
            DropIndex("dbo.OnlineCourse", new[] { "SessionID" });
            DropIndex("dbo.OnlineCourse", new[] { "OnlineProviderID" });
            DropIndex("dbo.OnlineCourse", new[] { "CategoryID" });
            DropTable("dbo.Student");
            DropTable("dbo.Provider");
            DropTable("dbo.Primary");
            DropTable("dbo.Parent");
            DropTable("dbo.CourseCredit");
            DropTable("dbo.CourseCategory");
            DropTable("dbo.Counselor");
            DropTable("dbo.Session");
            DropTable("dbo.OnlineProvider");
            DropTable("dbo.Course");
            DropTable("dbo.Category");
            DropTable("dbo.OnlineCourse");
            DropTable("dbo.ClassSession");
        }
    }
}
