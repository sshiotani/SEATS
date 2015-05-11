namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CCA",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SubmitterTypeID = c.Int(),
                        UserId = c.String(),
                        ApplicationSubmissionDate = c.DateTime(),
                        AssessedProficiency = c.String(),
                        StudentID = c.Int(nullable: false),
                        StudentGradeLevel = c.Int(),
                        HasExcessiveFED = c.Boolean(nullable: false),
                        ExcessiveFEDExplanation = c.String(),
                        ExcessiveFEDReasonCode = c.Int(),
                        Comments = c.String(),
                        CounselorID = c.Int(nullable: false),
                        IsCounselorSigned = c.Boolean(nullable: false),
                        PrimaryID = c.Int(nullable: false),
                        IsBusinessAdministratorAcceptRejectEnrollment = c.Boolean(nullable: false),
                        PrimaryLEAExplantionRejection = c.String(),
                        PrimaryLEAReasonRejectingCCA = c.String(),
                        ProviderID = c.Int(nullable: false),
                        TeacherCactusID = c.Int(),
                        TeacherFirstName = c.String(),
                        TeacherLastName = c.String(),
                        CompletionStatus = c.String(),
                        CourseBegin = c.DateTime(),
                        CourseStartDate = c.DateTime(),
                        CourseCompletionDate = c.DateTime(),
                        CreditCompletedToDate = c.Decimal(precision: 18, scale: 2),
                        DateConfirmationActiveParticipation = c.DateTime(),
                        DateContinuationActiveParticipation = c.DateTime(),
                        DateReportPassingGrade = c.DateTime(),
                        IsEnrollmentNoticeSent = c.Boolean(nullable: false),
                        IsProviderAcceptsRejectsCourseRequest = c.Boolean(nullable: false),
                        IsProviderEnrollmentVerified = c.Boolean(nullable: false),
                        IsProviderSignature = c.Boolean(nullable: false),
                        ProviderExplanationRejection = c.String(),
                        ProviderReasonRejection = c.String(),
                        CourseID = c.Int(nullable: false),
                        CourseCategoryID = c.Int(),
                        CourseFee = c.Decimal(precision: 18, scale: 2),
                        CourseCreditID = c.Int(),
                        CourseName2ndSemesterID = c.Int(),
                        IsCourseConsistentWithStudentSEOP = c.Boolean(nullable: false),
                        SessionID = c.Int(nullable: false),
                        BudgetPrimaryProvider = c.Decimal(precision: 18, scale: 2),
                        IsRemediation = c.Boolean(nullable: false),
                        NotificationDate = c.DateTime(),
                        PrimaryNotificationDate = c.DateTime(),
                        PriorDisbursementProvider = c.Decimal(precision: 18, scale: 2),
                        RecordNotes = c.String(),
                        RemediationPeriodBegins = c.DateTime(),
                        TotalDisbursementsProvider = c.Decimal(precision: 18, scale: 2),
                        TwentyDaysPastSemesterStartDate = c.DateTime(),
                        Unallocated = c.Decimal(precision: 18, scale: 2),
                        UnallocatedReduction = c.Decimal(precision: 18, scale: 2),
                        WithdrawalDate = c.DateTime(),
                        Grand_Total = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Counselor", t => t.CounselorID, cascadeDelete: true)
                .ForeignKey("dbo.OnlineCourse", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.CourseCredit", t => t.CourseCreditID)
                .ForeignKey("dbo.Primary", t => t.PrimaryID, cascadeDelete: true)
                .ForeignKey("dbo.Provider", t => t.ProviderID, cascadeDelete: true)
                .ForeignKey("dbo.Student", t => t.StudentID, cascadeDelete: true)
                .Index(t => t.StudentID)
                .Index(t => t.CounselorID)
                .Index(t => t.PrimaryID)
                .Index(t => t.ProviderID)
                .Index(t => t.CourseID)
                .Index(t => t.CourseCreditID);
            
            CreateTable(
                "dbo.Counselor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CounselorCactusID = c.Int(),
                        CounselorEmail = c.String(),
                        CounselorFirstName = c.String(),
                        CounselorLastName = c.String(),
                        School = c.String(),
                        CounselorPhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OnlineCourse",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Credit = c.String(),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Notes = c.String(),
                        CourseCategoryID = c.Int(nullable: false),
                        ProviderID = c.Int(nullable: false),
                        SessionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CourseCategory", t => t.CourseCategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Provider", t => t.ProviderID, cascadeDelete: true)
                .ForeignKey("dbo.Session", t => t.SessionID, cascadeDelete: true)
                .Index(t => t.CourseCategoryID)
                .Index(t => t.ProviderID)
                .Index(t => t.SessionID);
            
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
                "dbo.Session",
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
                "dbo.Category",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
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
                "dbo.CourseCredit",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Value = c.String(),
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
                "dbo.Student",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ParentID = c.Int(),
                        StudentFirstName = c.String(),
                        StudentLastName = c.String(),
                        SSID = c.Int(),
                        StudentDOB = c.DateTime(),
                        StudentEmail = c.String(),
                        EnrollmentLocationID = c.Int(),
                        EnrollmentLocationSchoolNamesID = c.Int(),
                        SchoolOfRecord = c.String(),
                        GraduationDate = c.DateTime(),
                        HasHomeSchoolRelease = c.Boolean(nullable: false),
                        IsEarlyGraduate = c.Boolean(nullable: false),
                        IsFeeWaived = c.Boolean(nullable: false),
                        IsIEP = c.Boolean(nullable: false),
                        IsPrimaryEnrollmentVerified = c.Boolean(nullable: false),
                        IsSection504 = c.Boolean(nullable: false),
                        StudentBudgetID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Parent",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GuardianEmail = c.String(nullable: false),
                        GuardianFirstName = c.String(nullable: false),
                        GuardianLastName = c.String(nullable: false),
                        GuardianPhone1 = c.String(),
                        GuardianPhone2 = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CCA", "StudentID", "dbo.Student");
            DropForeignKey("dbo.CCA", "ProviderID", "dbo.Provider");
            DropForeignKey("dbo.CCA", "PrimaryID", "dbo.Primary");
            DropForeignKey("dbo.CCA", "CourseCreditID", "dbo.CourseCredit");
            DropForeignKey("dbo.CCA", "CourseID", "dbo.OnlineCourse");
            DropForeignKey("dbo.OnlineCourse", "SessionID", "dbo.Session");
            DropForeignKey("dbo.Course", "SessionID", "dbo.Session");
            DropForeignKey("dbo.Course", "OnlineProviderID", "dbo.OnlineProvider");
            DropForeignKey("dbo.Course", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.OnlineCourse", "ProviderID", "dbo.Provider");
            DropForeignKey("dbo.OnlineCourse", "CourseCategoryID", "dbo.CourseCategory");
            DropForeignKey("dbo.CCA", "CounselorID", "dbo.Counselor");
            DropIndex("dbo.Course", new[] { "SessionID" });
            DropIndex("dbo.Course", new[] { "OnlineProviderID" });
            DropIndex("dbo.Course", new[] { "CategoryID" });
            DropIndex("dbo.OnlineCourse", new[] { "SessionID" });
            DropIndex("dbo.OnlineCourse", new[] { "ProviderID" });
            DropIndex("dbo.OnlineCourse", new[] { "CourseCategoryID" });
            DropIndex("dbo.CCA", new[] { "CourseCreditID" });
            DropIndex("dbo.CCA", new[] { "CourseID" });
            DropIndex("dbo.CCA", new[] { "ProviderID" });
            DropIndex("dbo.CCA", new[] { "PrimaryID" });
            DropIndex("dbo.CCA", new[] { "CounselorID" });
            DropIndex("dbo.CCA", new[] { "StudentID" });
            DropTable("dbo.Parent");
            DropTable("dbo.Student");
            DropTable("dbo.Primary");
            DropTable("dbo.CourseCredit");
            DropTable("dbo.OnlineProvider");
            DropTable("dbo.Category");
            DropTable("dbo.Course");
            DropTable("dbo.Session");
            DropTable("dbo.Provider");
            DropTable("dbo.CourseCategory");
            DropTable("dbo.OnlineCourse");
            DropTable("dbo.Counselor");
            DropTable("dbo.CCA");
        }
    }
}
