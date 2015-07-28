using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    /// <summary>
    /// This class is used to show the cca report before it is downloaded in Excel.
    /// </summary>
    public class ReportViewModel
    {
        public int Id { get; set; }
        public string FiscalYear { get; set; }
        public string SSID { get; set; }
        public string CourseCredit { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }    
        public string EnrollmentLocation { get; set; }
        public string Provider { get; set; }
        public decimal CourseFee { get; set; }
        public Nullable<decimal> BudgetPrimaryProvider { get; set; }
        public Nullable<decimal> PriorDisbursementProvider { get; set; }
        public decimal Offset { get; set; }
        public decimal Distribution { get; set; }
        public Nullable<decimal> TotalDisbursementsProvider { get; set; }
        public string CourseCategory { get; set; }    
        public string OnlineCourse { get; set; }
        public Nullable<int> CourseCompletionStatusID { get; set; }
        public CourseCompletionStatus CourseCompletionStatus { get; set; }
        public bool IsBusinessAdministratorAcceptRejectEnrollment { get; set; }
        public Nullable<System.DateTime> CourseStartDate { get; set; }
        public string ParentEmail { get; set; }
        public string CounselorEmail { get; set; }
    }
}