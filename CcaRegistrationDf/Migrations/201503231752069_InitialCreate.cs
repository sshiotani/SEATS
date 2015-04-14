namespace CcaRegistrationDf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MainTables",
                c => new
                    {
                        AgreementID = c.Int(nullable: false, identity: true),
                        ApplicationSubmissionDate = c.DateTime(),
                        AssessedProficiency = c.String(),
                        BudgetPrimaryProvider = c.Decimal(precision: 18, scale: 2),
                        Business_Administrator_Email = c.String(),
                        BusinessAdministratorFirstName = c.String(),
                        BusinessAdministratorLastName = c.String(),
                        BusinessAdministratorSignature = c.Boolean(nullable: false),
                        BusinessAdministratorTelephone = c.String(),
                        Comments = c.String(),
                        CompletionStatus = c.String(),
                        CounselorCactusID = c.Int(),
                        CounselorEmail = c.String(),
                        CounselorFirstName = c.String(),
                        CounselorLastName = c.String(),
                        CounselorPhoneNumber = c.String(),
                        CourseBegin = c.DateTime(),
                        CourseCompletionDate = c.DateTime(),
                        CourseCreditID = c.Int(),
                        CourseFee = c.Decimal(precision: 18, scale: 2),
                        CourseID = c.Int(),
                        CourseRecord2ndSemesterID = c.String(),
                        CourseRecordID = c.String(),
                        CourseStartDate = c.DateTime(),
                        CreditCompletedToDate = c.Decimal(precision: 18, scale: 2),
                        DateBusinessAdministratorSignature = c.DateTime(),
                        DateConfirmationActiveParticipation = c.DateTime(),
                        DateContinuationActiveParticipation = c.DateTime(),
                        DateReportPassingGrade = c.DateTime(),
                        EnrollmentLocationID = c.Int(),
                        ExcessiveFEDExplanation = c.String(),
                        ExcessiveFEDReasonCode = c.Int(),
                        GraduationDate = c.DateTime(),
                        GuardianEmail = c.String(),
                        GuardianFirstName = c.String(),
                        GuardianLastName = c.String(),
                        GuardianPhone1 = c.String(),
                        GuardianPhone2 = c.String(),
                        IsBusinessAdministratorAcceptRejectEnrollment = c.Boolean(nullable: false),
                        IsCounselorSigned = c.Boolean(nullable: false),
                        IsCourseConsistentWithStudentSEOP = c.Boolean(nullable: false),
                        IsEarlyGraduate = c.Boolean(nullable: false),
                        IsEnrollmentNoticeSent = c.Boolean(nullable: false),
                        IsFeeWaived = c.Boolean(nullable: false),
                        IsGuardianSigned = c.Boolean(nullable: false),
                        IsIEP = c.Boolean(nullable: false),
                        IsPrimaryEnrollmentVerified = c.Boolean(nullable: false),
                        IsProviderAcceptsRejectsCourseRequest = c.Boolean(nullable: false),
                        IsProviderEnrollmentVerified = c.Boolean(nullable: false),
                        IsProviderSignature = c.Boolean(nullable: false),
                        IsQ1 = c.Boolean(nullable: false),
                        IsQ2 = c.Boolean(nullable: false),
                        IsQ3 = c.Boolean(nullable: false),
                        IsQ4 = c.Boolean(nullable: false),
                        IsRemediation = c.Boolean(nullable: false),
                        IsSection504 = c.Boolean(nullable: false),
                        IsStudentSigned = c.Boolean(nullable: false),
                        NotificationDate = c.DateTime(),
                        NovemberFY15Distr = c.Decimal(precision: 18, scale: 2),
                        NovemberFY15Offset = c.Decimal(precision: 18, scale: 2),
                        OnlineProviderID = c.Int(),
                        ParentTelephone2 = c.String(),
                        PricingTier = c.Int(),
                        PrimaryLEAExplantionRejection = c.String(),
                        PrimaryLEAReasonRejectingCCA = c.String(),
                        PrimaryNotificationDate = c.DateTime(),
                        PriorDisbursementProvider = c.Decimal(precision: 18, scale: 2),
                        ProviderEmail = c.String(),
                        ProviderExplanationRejection = c.String(),
                        ProviderFax = c.String(),
                        ProviderFirstName = c.String(),
                        ProviderLastName = c.String(),
                        ProviderPhoneNumber = c.String(),
                        ProviderReasonRejection = c.String(),
                        RecordNotes = c.String(),
                        RemediationPeriodBegins = c.DateTime(),
                        SchoolOfRecord = c.String(),
                        SSID = c.Int(),
                        StartDateSecondSemester = c.DateTime(),
                        StudentDOB = c.DateTime(),
                        StudentEmail = c.String(),
                        StudentFirstName = c.String(),
                        StudentGradeLevel = c.Int(),
                        StudentLastName = c.String(),
                        SubmissionDate = c.DateTime(),
                        SubmitterTypeID = c.Int(),
                        TeacherCactusID = c.Int(),
                        TotalDisbursementsProvider = c.Decimal(precision: 18, scale: 2),
                        TwentyDaysPastSemesterStartDate = c.DateTime(),
                        Unallocated = c.Decimal(precision: 18, scale: 2),
                        UnallocatedReduction = c.Decimal(precision: 18, scale: 2),
                        WithdrawalDate = c.DateTime(),
                        Grand_Total = c.String(),
                    })
                .PrimaryKey(t => t.AgreementID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.MainTables");
        }
    }
}
