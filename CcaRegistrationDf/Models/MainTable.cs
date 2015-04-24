//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CcaRegistrationDf.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class MainTable
    {
        [Key]
        public int AgreementID { get; set; }
        public Nullable<System.DateTime> ApplicationSubmissionDate { get; set; }
        public string AssessedProficiency { get; set; }      
        public Nullable<int> SubmitterTypeID { get; set; }

        //Student 
        public string Comments { get; set; }
        public Nullable<int> EnrollmentLocationID { get; set; }
        public bool HasExcessiveFED { get; set; }
        public string ExcessiveFEDExplanation { get; set; }
        public Nullable<int> ExcessiveFEDReasonCode { get; set; }
        public Nullable<System.DateTime> GraduationDate { get; set; }
        public bool IsEarlyGraduate { get; set; }
        public bool IsFeeWaived { get; set; }
        public bool IsIEP { get; set; }
        public bool IsPrimaryEnrollmentVerified { get; set; }
        public bool IsSection504 { get; set; }
        public bool IsStudentSigned { get; set; }
        public string SchoolOfRecord { get; set; }
        public Nullable<int> SSID { get; set; }
        public Nullable<System.DateTime> StudentDOB { get; set; }
        public string StudentEmail { get; set; }
        public string StudentFirstName { get; set; }
        public Nullable<int> StudentGradeLevel { get; set; }
        public string StudentLastName { get; set; }


        //Parent 
        public string GuardianEmail { get; set; }
        public string GuardianFirstName { get; set; }
        public string GuardianLastName { get; set; }
        public string GuardianPhone1 { get; set; }
        public string GuardianPhone2 { get; set; }
        public bool IsGuardianSigned { get; set; }
       
        //Counselor 
        public Nullable<int> CounselorCactusID { get; set; }
        public string CounselorEmail { get; set; }
        public string CounselorFirstName { get; set; }
        public string CounselorLastName { get; set; }
        public string CounselorPhoneNumber { get; set; }
        public bool IsCounselorSigned { get; set; }

        //Course
        public Nullable<int> CourseCategoryID { get; set; }
        public Nullable<decimal> CourseFee { get; set; }
        public Nullable<int> CourseNameID { get; set; }
        public Nullable<int> CourseCreditID { get; set; }
        public Nullable<int> CourseName2ndSemesterID { get; set; }
        public bool IsCourseConsistentWithStudentSEOP { get; set; }
        public Nullable<int> OnlineProviderID { get; set; }
        public Nullable<int> PricingTier { get; set; }
        public int SemesterID { get; set; }
        public Nullable<System.DateTime> StartDateSecondSemester { get; set; }


        //Provider
        
        public string CompletionStatus { get; set; }
        public Nullable<System.DateTime> CourseBegin { get; set; }
        public Nullable<System.DateTime> CourseStartDate { get; set; }
        public Nullable<System.DateTime> CourseCompletionDate { get; set; }
        public Nullable<decimal> CreditCompletedToDate { get; set; }
        public Nullable<System.DateTime> DateConfirmationActiveParticipation { get; set; }
        public Nullable<System.DateTime> DateContinuationActiveParticipation { get; set; }
        public Nullable<System.DateTime> DateReportPassingGrade { get; set; }
        public bool IsEnrollmentNoticeSent { get; set; }
        public bool IsProviderAcceptsRejectsCourseRequest { get; set; }
        public bool IsProviderEnrollmentVerified { get; set; }
        public bool IsProviderSignature { get; set; }
        public string ProviderEmail { get; set; }
        public string ProviderExplanationRejection { get; set; }
        public string ProviderFax { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderLastName { get; set; }
        public string ProviderPhoneNumber { get; set; }
        public string ProviderReasonRejection { get; set; }
        public Nullable<int> ProviderCactusID { get; set; }

        // Credit

        // Primary
        public string Business_Administrator_Email { get; set; }
        public string BusinessAdministratorFirstName { get; set; }
        public string BusinessAdministratorLastName { get; set; }
        public bool BusinessAdministratorSignature { get; set; }
        public string BusinessAdministratorTelephone { get; set; }
        public Nullable<System.DateTime> DateBusinessAdministratorSignature { get; set; }
        public bool IsBusinessAdministratorAcceptRejectEnrollment { get; set; }
        public string PrimaryLEAExplantionRejection { get; set; }
        public string PrimaryLEAReasonRejectingCCA { get; set; }
        
  
        //USOE
        public Nullable<decimal> BudgetPrimaryProvider { get; set; }
        public bool IsRemediation { get; set; }
        public Nullable<System.DateTime> NotificationDate { get; set; }
        public Nullable<decimal> NovemberFY15Distr { get; set; }
        public Nullable<decimal> NovemberFY15Offset { get; set; }
        public Nullable<System.DateTime> PrimaryNotificationDate { get; set; }
        public Nullable<decimal> PriorDisbursementProvider { get; set; }
        public string RecordNotes { get; set; }
        public Nullable<System.DateTime> RemediationPeriodBegins { get; set; }
        public Nullable<decimal> TotalDisbursementsProvider { get; set; }
        public Nullable<System.DateTime> TwentyDaysPastSemesterStartDate { get; set; } 
        public Nullable<decimal> Unallocated { get; set; }
        public Nullable<decimal> UnallocatedReduction { get; set; }
        public Nullable<System.DateTime> WithdrawalDate { get; set; }
        public Nullable<decimal> Grand_Total { get; set; }
    }
}