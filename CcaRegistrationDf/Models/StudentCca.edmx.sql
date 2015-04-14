
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/31/2015 14:20:04
-- Generated from EDMX file: C:\Users\sshiotani\documents\visual studio 2013\Projects\CcaRegistrationDf\CcaRegistrationDf\Models\StudentCca.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SEATS];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[CCA].[FK_Course_Category]', 'F') IS NOT NULL
    ALTER TABLE [CCA].[Course] DROP CONSTRAINT [FK_Course_Category];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[CCA].[Category]', 'U') IS NOT NULL
    DROP TABLE [CCA].[Category];
GO
IF OBJECT_ID(N'[CCA].[Course]', 'U') IS NOT NULL
    DROP TABLE [CCA].[Course];
GO
IF OBJECT_ID(N'[CCA].[CourseCredit]', 'U') IS NOT NULL
    DROP TABLE [CCA].[CourseCredit];
GO
IF OBJECT_ID(N'[CCA].[EnrollmentLocation]', 'U') IS NOT NULL
    DROP TABLE [CCA].[EnrollmentLocation];
GO
IF OBJECT_ID(N'[CCA].[MainTable]', 'U') IS NOT NULL
    DROP TABLE [CCA].[MainTable];
GO
IF OBJECT_ID(N'[CCA].[OnlineProvider]', 'U') IS NOT NULL
    DROP TABLE [CCA].[OnlineProvider];
GO
IF OBJECT_ID(N'[CCA].[SchoolYearSession]', 'U') IS NOT NULL
    DROP TABLE [CCA].[SchoolYearSession];
GO
IF OBJECT_ID(N'[CCA].[Session]', 'U') IS NOT NULL
    DROP TABLE [CCA].[Session];
GO
IF OBJECT_ID(N'[CCA].[SubmitterType]', 'U') IS NOT NULL
    DROP TABLE [CCA].[SubmitterType];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(60)  NOT NULL
);
GO

-- Creating table 'Courses'
CREATE TABLE [dbo].[Courses] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CategoryID] int  NOT NULL,
    [Name] nvarchar(60)  NOT NULL,
    [Credit] decimal(18,0)  NULL,
    [Code] nvarchar(50)  NULL,
    [OnlineProviderID] int  NOT NULL
);
GO

-- Creating table 'CourseCredits'
CREATE TABLE [dbo].[CourseCredits] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(60)  NOT NULL
);
GO

-- Creating table 'EnrollmentLocations'
CREATE TABLE [dbo].[EnrollmentLocations] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(60)  NOT NULL
);
GO

-- Creating table 'MainTables'
CREATE TABLE [dbo].[MainTables] (
    [AgreementID] int  NOT NULL,
    [ApplicationSubmissionDate] datetime  NULL,
    [AssessedProficiency] nvarchar(20)  NULL,
    [BudgetPrimaryProvider] decimal(19,4)  NULL,
    [Business_Administrator_Email] varchar(30)  NULL,
    [BusinessAdministratorFirstName] nvarchar(100)  NULL,
    [BusinessAdministratorLastName] nvarchar(100)  NULL,
    [BusinessAdministratorSignature] bit  NOT NULL,
    [BusinessAdministratorTelephone] varchar(10)  NULL,
    [Comments] nvarchar(max)  NOT NULL,
    [CompletionStatus] nvarchar(20)  NULL,
    [CounselorCactusID] int  NULL,
    [CounselorEmail] varchar(30)  NULL,
    [CounselorFirstName] nvarchar(100)  NULL,
    [CounselorLastName] nvarchar(100)  NULL,
    [CounselorPhoneNumber] varchar(10)  NOT NULL,
    [CourseBegin] datetime  NULL,
    [CourseCompletionDate] datetime  NULL,
    [CourseCreditID] int  NULL,
    [CourseFee] decimal(19,4)  NULL,
    [CourseID] int  NULL,
    [CourseRecord2ndSemesterID] nvarchar(20)  NULL,
    [CourseRecordID] nvarchar(20)  NULL,
    [CourseStartDate] datetime  NULL,
    [CreditCompletedToDate] decimal(38,0)  NULL,
    [DateBusinessAdministratorSignature] datetime  NULL,
    [DateConfirmationActiveParticipation] datetime  NULL,
    [DateContinuationActiveParticipation] datetime  NULL,
    [DateReportPassingGrade] datetime  NULL,
    [EnrollmentLocationID] int  NULL,
    [ExcessiveFEDExplanation] nvarchar(255)  NULL,
    [ExcessiveFEDReasonCode] int  NULL,
    [GraduationDate] datetime  NULL,
    [GuardianEmail] varchar(30)  NULL,
    [GuardianFirstName] nvarchar(100)  NULL,
    [GuardianLastName] nvarchar(100)  NULL,
    [GuardianPhone1] varchar(10)  NULL,
    [GuardianPhone2] varchar(10)  NULL,
    [IsBusinessAdministratorAcceptRejectEnrollment] bit  NOT NULL,
    [IsCounselorSigned] bit  NOT NULL,
    [IsCourseConsistentWithStudentSEOP] bit  NOT NULL,
    [IsEarlyGraduate] bit  NOT NULL,
    [IsEnrollmentNoticeSent] bit  NOT NULL,
    [IsFeeWaived] bit  NOT NULL,
    [IsGuardianSigned] bit  NOT NULL,
    [IsIEP] bit  NOT NULL,
    [IsPrimaryEnrollmentVerified] bit  NOT NULL,
    [IsProviderAcceptsRejectsCourseRequest] bit  NOT NULL,
    [IsProviderEnrollmentVerified] bit  NOT NULL,
    [IsProviderSignature] bit  NOT NULL,
    [IsQ1] bit  NOT NULL,
    [IsQ2] bit  NOT NULL,
    [IsQ3] bit  NOT NULL,
    [IsQ4] bit  NOT NULL,
    [IsRemediation] bit  NOT NULL,
    [IsSection504] bit  NOT NULL,
    [IsStudentSigned] bit  NOT NULL,
    [NotificationDate] datetime  NULL,
    [NovemberFY15Distr] decimal(19,4)  NULL,
    [NovemberFY15Offset] decimal(19,4)  NULL,
    [OnlineProviderID] int  NULL,
    [ParentTelephone2] varchar(10)  NULL,
    [PricingTier] int  NULL,
    [PrimaryLEAExplantionRejection] nvarchar(255)  NULL,
    [PrimaryLEAReasonRejectingCCA] nvarchar(255)  NULL,
    [PrimaryNotificationDate] datetime  NULL,
    [PriorDisbursementProvider] decimal(19,4)  NULL,
    [ProviderEmail] varchar(30)  NULL,
    [ProviderExplanationRejection] nvarchar(255)  NULL,
    [ProviderFax] varchar(10)  NULL,
    [ProviderFirstName] nvarchar(100)  NULL,
    [ProviderLastName] nvarchar(100)  NULL,
    [ProviderPhoneNumber] varchar(10)  NULL,
    [ProviderReasonRejection] nvarchar(255)  NULL,
    [RecordNotes] nvarchar(max)  NULL,
    [RemediationPeriodBegins] datetime  NULL,
    [SchoolOfRecord] nvarchar(128)  NULL,
    [SSID] int  NULL,
    [StartDateSecondSemester] datetime  NULL,
    [StudentDOB] datetime  NULL,
    [StudentEmail] varchar(30)  NULL,
    [StudentFirstName] nvarchar(100)  NULL,
    [StudentGradeLevel] int  NULL,
    [StudentLastName] nvarchar(100)  NULL,
    [SubmissionDate] datetime  NULL,
    [SubmitterTypeID] int  NULL,
    [TeacherCactusID] int  NULL,
    [TotalDisbursementsProvider] decimal(19,4)  NULL,
    [TwentyDaysPastSemesterStartDate] datetime  NULL,
    [Unallocated] decimal(19,4)  NULL,
    [UnallocatedReduction] decimal(19,4)  NULL,
    [WithdrawalDate] datetime  NULL,
    [Grand_Total] nvarchar(255)  NULL
);
GO

-- Creating table 'OnlineProviders'
CREATE TABLE [dbo].[OnlineProviders] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(60)  NOT NULL,
    [DistrictID] varchar(5)  NULL
);
GO

-- Creating table 'SchoolYearSessions'
CREATE TABLE [dbo].[SchoolYearSessions] (
    [SchoolYear] datetime  NOT NULL,
    [SessionID] int  NOT NULL
);
GO

-- Creating table 'Sessions'
CREATE TABLE [dbo].[Sessions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(30)  NOT NULL
);
GO

-- Creating table 'SubmitterTypes'
CREATE TABLE [dbo].[SubmitterTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(30)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID], [CategoryID] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [PK_Courses]
    PRIMARY KEY CLUSTERED ([ID], [CategoryID] ASC);
GO

-- Creating primary key on [ID] in table 'CourseCredits'
ALTER TABLE [dbo].[CourseCredits]
ADD CONSTRAINT [PK_CourseCredits]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EnrollmentLocations'
ALTER TABLE [dbo].[EnrollmentLocations]
ADD CONSTRAINT [PK_EnrollmentLocations]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [AgreementID] in table 'MainTables'
ALTER TABLE [dbo].[MainTables]
ADD CONSTRAINT [PK_MainTables]
    PRIMARY KEY CLUSTERED ([AgreementID] ASC);
GO

-- Creating primary key on [ID] in table 'OnlineProviders'
ALTER TABLE [dbo].[OnlineProviders]
ADD CONSTRAINT [PK_OnlineProviders]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [SchoolYear], [SessionID] in table 'SchoolYearSessions'
ALTER TABLE [dbo].[SchoolYearSessions]
ADD CONSTRAINT [PK_SchoolYearSessions]
    PRIMARY KEY CLUSTERED ([SchoolYear], [SessionID] ASC);
GO

-- Creating primary key on [ID] in table 'Sessions'
ALTER TABLE [dbo].[Sessions]
ADD CONSTRAINT [PK_Sessions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SubmitterTypes'
ALTER TABLE [dbo].[SubmitterTypes]
ADD CONSTRAINT [PK_SubmitterTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CategoryID] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [FK_Course_Category]
    FOREIGN KEY ([CategoryID])
    REFERENCES [dbo].[Categories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Course_Category'
CREATE INDEX [IX_FK_Course_Category]
ON [dbo].[Courses]
    ([CategoryID]);
GO

-- Creating foreign key on [OnlineProviderID] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [FK_OnlineProviderCourse]
    FOREIGN KEY ([OnlineProviderID])
    REFERENCES [dbo].[OnlineProviders]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OnlineProviderCourse'
CREATE INDEX [IX_FK_OnlineProviderCourse]
ON [dbo].[Courses]
    ([OnlineProviderID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------