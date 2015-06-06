using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class UsoeCcaViewModel
    {
        // Items pulled from CCA
        public DateTime ApplicationSubmissionDate { get; set; }
        public int CcaID { get; set; }
        public int StudentID { get; set; }
        public Student Student { get; set; }
        public int OnlineCourseID { get; set; }
        public OnlineCourse OnlineCourse { get; set; }
        public int CourseCreditID { get; set; }
        public CourseCredit CourseCredit { get; set; }

        //Items to be input by USOE 
        public Nullable<decimal> CourseFee { get; set; }
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
    }
}