namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeSOEPContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CCAs",
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
                .ForeignKey("dbo.Counselors", t => t.CounselorID, cascadeDelete: false)
                .ForeignKey("dbo.OnlineCourses", t => t.CourseID, cascadeDelete: false)
                .ForeignKey("dbo.CourseCredits", t => t.CourseCreditID)
                .ForeignKey("dbo.Primaries", t => t.PrimaryID, cascadeDelete: false)
                .ForeignKey("dbo.Providers", t => t.ProviderID, cascadeDelete: false)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: false)
                .Index(t => t.StudentID)
                .Index(t => t.CounselorID)
                .Index(t => t.PrimaryID)
                .Index(t => t.ProviderID)
                .Index(t => t.CourseID)
                .Index(t => t.CourseCreditID);
            
            CreateTable(
                "dbo.Counselors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CactusID = c.Int(),
                        CounselorEmail = c.String(),
                        CounselorFirstName = c.String(),
                        CounselorLastName = c.String(),
                        School = c.String(),
                        CounselorPhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OnlineCourses",
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
                .ForeignKey("dbo.CourseCategories", t => t.CourseCategoryID, cascadeDelete: false)
                .ForeignKey("dbo.Providers", t => t.ProviderID, cascadeDelete: false)
                .ForeignKey("dbo.Sessions", t => t.SessionID, cascadeDelete: false)
                .Index(t => t.CourseCategoryID)
                .Index(t => t.ProviderID)
                .Index(t => t.SessionID);
            
            CreateTable(
                "dbo.CourseCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Providers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DistrictNumber = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        ProviderFirstName = c.String(),
                        ProviderLastName = c.String(),
                        ProviderPhoneNumber = c.String(),
                        ProviderEmail = c.String(),
                        ProviderFax = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CourseCredits",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Primaries",
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
                "dbo.Students",
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
                "dbo.Parents",
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
            DropForeignKey("dbo.CCAs", "StudentID", "dbo.Students");
            DropForeignKey("dbo.CCAs", "ProviderID", "dbo.Providers");
            DropForeignKey("dbo.CCAs", "PrimaryID", "dbo.Primaries");
            DropForeignKey("dbo.CCAs", "CourseCreditID", "dbo.CourseCredits");
            DropForeignKey("dbo.CCAs", "CourseID", "dbo.OnlineCourses");
            DropForeignKey("dbo.OnlineCourses", "SessionID", "dbo.Sessions");
            DropForeignKey("dbo.OnlineCourses", "ProviderID", "dbo.Providers");
            DropForeignKey("dbo.OnlineCourses", "CourseCategoryID", "dbo.CourseCategories");
            DropForeignKey("dbo.CCAs", "CounselorID", "dbo.Counselors");
            DropIndex("dbo.OnlineCourses", new[] { "SessionID" });
            DropIndex("dbo.OnlineCourses", new[] { "ProviderID" });
            DropIndex("dbo.OnlineCourses", new[] { "CourseCategoryID" });
            DropIndex("dbo.CCAs", new[] { "CourseCreditID" });
            DropIndex("dbo.CCAs", new[] { "CourseID" });
            DropIndex("dbo.CCAs", new[] { "ProviderID" });
            DropIndex("dbo.CCAs", new[] { "PrimaryID" });
            DropIndex("dbo.CCAs", new[] { "CounselorID" });
            DropIndex("dbo.CCAs", new[] { "StudentID" });
            DropTable("dbo.Parents");
            DropTable("dbo.Students");
            DropTable("dbo.Primaries");
            DropTable("dbo.CourseCredits");
            DropTable("dbo.Sessions");
            DropTable("dbo.Providers");
            DropTable("dbo.CourseCategories");
            DropTable("dbo.OnlineCourses");
            DropTable("dbo.Counselors");
            DropTable("dbo.CCAs");
        }
    }
}
