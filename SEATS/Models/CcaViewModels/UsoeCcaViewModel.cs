using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEATS.Models
{
    public class UsoeCcaViewModel : CCAViewModel
    {
        // Items pulled from CCA
        public DateTime ApplicationSubmissionDate { get; set; }
        public int CcaID { get; set; }
        //public int ProviderID { get; set; }
        public Provider Provider { get; set; }
        public int StudentID { get; set; }
        public Student Student { get; set; }
        //public int SessionID { get; set; }
        public Session Session { get; set; }
        //public int CourseCategoryID { get; set; }
        public CourseCategory CourseCategory { get; set; }
        //public int OnlineCourseID { get; set; }
        public OnlineCourse OnlineCourse { get; set; }
       // public int CourseCreditID { get; set; }
        public CourseCredit CourseCredit { get; set; }
        //public IEnumerable<SelectListItem> CourseCreditList { get; set; }
        public string Primary { get; set; }
        public bool IsBusinessAdministratorAcceptRejectEnrollment { get; set; }
        public int MyProperty { get; set; }
        public bool IsProviderAcceptsRejectsCourseRequest { get; set; }

        //public int? StudentGradeLevel { get; set; }

        //public int? EnrollmentLocationID { get; set; }
        public int? EnrollmentLocationSchoolNamesID { get; set; }
        //public string SchoolOfRecord { get; set; }

        //Counselor
        //public Nullable<int> CounselorID { get; set; }
        //public virtual Counselor Counselor { get; set; }
        //public IEnumerable<SelectListItem> CounselorList { get; set; }
        //public Nullable<int> CactusID { get; set; }
        //public bool IsCounselorSigned { get; set; }

        //[EmailAddress]
        //[Display(Name = "Counselor Email")]
        //public string CounselorEmail { get; set; }

        //[Display(Name = "Counselor First Name")]
        //public string CounselorFirstName { get; set; }

        //[Display(Name = "Student Last Name")]
        //public string CounselorLastName { get; set; }

        //public string CounselorPhoneNumber { get; set; }

        //Items to be input by USOE 
        //public Nullable<decimal> CourseFee { get; set; }
        public Nullable<int> CourseCompletionStatusID { get; set; }
        public CourseCompletionStatus CourseCompletionStatus { get; set; }
        public Nullable<decimal> BudgetPrimaryProvider { get; set; }
        public bool IsRemediation { get; set; }
        public bool IsEnrollmentNoticeSent { get; set; }
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
        
        // Added for uploading of existing CCAs.
        [Display(Name = "Teacher Cactus ID")]
        public Nullable<int> TeacherCactusID { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
    }
}