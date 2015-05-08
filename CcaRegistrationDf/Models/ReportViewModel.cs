using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class ReportViewModel
    {
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public Nullable<int> SSID { get; set; }
        public Nullable<System.DateTime> StudentDOB { get; set; }

        public Nullable<int> CreditID { get; set; }
        public string Credit { get; set; }
        public Nullable<int> OnlineProviderID { get; set; }
        public string OnlineProvider { get; set; }
        public Nullable<int> EnrollmentLocationID { get; set; }
        public string EnrollmentLocation { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string Category { get; set; }
        public Nullable<int> CourseID { get; set; }
        public string Course { get; set; }
        public Nullable<decimal> BudgetPrimaryProvider { get; set; }
        public Nullable<decimal> PriorDisbursementProvider { get; set; }
        public Nullable<int> StudentMonthlyBudgetID { get; set; }  
        public Nullable<decimal> TotalDisbursementsProvider { get; set; }
    }
}