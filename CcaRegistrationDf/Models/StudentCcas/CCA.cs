using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class CCA
    {
        
        public int ID { get; set; }

        public DateTime ApplicationSubmissionDate { get; set; }

        public string UserId { get; set; }

        //Student
        public int StudentID { get; set; }
        public virtual Student Student { get; set; }
        public Nullable<int> StudentGradeLevel { get; set; }
        public bool HasExcessiveFED { get; set; }
        public string ExcessiveFEDExplanation { get; set; }
        public Nullable<int> ExcessiveFEDReasonID { get; set; }
        public string Comments { get; set; }
        public Nullable<int> EnrollmentLocationSchoolNamesID { get; set; }

        //Counselor section
        public int CounselorID { get; set; }
        public virtual Counselor Counselor { get; set; }
        public bool IsCounselorSigned { get; set; }

        //Primary Section
        public Nullable<int> EnrollmentLocationID { get; set; }

        public Nullable<int> PrimaryID { get; set; }
        public virtual Primary Primary { get; set; }
        
        public bool IsBusinessAdministratorAcceptRejectEnrollment { get; set; }
        public string PrimaryLEAExplantionRejection { get; set; }
        public string PrimaryLEAReasonRejectingCCA { get; set; }
        public Nullable<System.DateTime> DateBusinessAdministratorSignature { get; set; }

        //Provider Section
        public int ProviderID { get; set; }
        public virtual Provider Provider { get; set; }
        [Display(Name = "Teacher Cactus ID")]
        public Nullable<int> TeacherCactusID { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
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
        public string ProviderExplanationRejection { get; set; }
        public string ProviderReasonRejection { get; set; }

        //Course
        public int OnlineCourseID { get; set; }
        public virtual OnlineCourse OnlineCourse { get; set; }
        public int CourseCategoryID { get; set; }
        public virtual CourseCategory CourseCategory { get; set; }
        //public Nullable<decimal> CourseFee { get; set; }
        public int CourseCreditID { get; set; }
        public virtual CourseCredit CourseCredit { get; set; }
        public Nullable<int> CourseName2ndSemesterID { get; set; }
        public bool IsCourseConsistentWithStudentSEOP { get; set; }      
        public int SessionID { get; set; }
        public Session Session { get; set; }

       

        //USOE Section

        public Nullable<decimal> BudgetPrimaryProvider { get; set; }
        public bool IsRemediation { get; set; }
        public Nullable<System.DateTime> NotificationDate { get; set; }
        public Nullable<System.DateTime> PrimaryNotificationDate { get; set; }
        public Nullable<decimal> PriorDisbursementProvider { get; set; }
        public string RecordNotes { get; set; }
        public Nullable<System.DateTime> RemediationPeriodBegins { get; set; }
        public Nullable<decimal> TotalDisbursementsProvider { get; set; }
        public Nullable<System.DateTime> TwentyDaysPastSemesterStartDate { get; set; }
        public Nullable<decimal> Offset { get; set; }
        public Nullable<decimal> Distribution { get; set; }
        public Nullable<System.DateTime> WithdrawalDate { get; set; }
        public Nullable<decimal> Grand_Total { get; set; }
        public string Notes { get; set; }
    }
}