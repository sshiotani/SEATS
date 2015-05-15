using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class ReportViewModel
    {
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
         
    }
}