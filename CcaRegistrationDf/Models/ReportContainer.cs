using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Model
{
    /// <summary>
    /// This class is neccessary to populate all the lists used for the DataSets in the report
    /// generator in ReportsController.  The report has multiple tables that provide data and
    /// each table needs a dataset list.  May look into creating a custom report view or table to 
    /// provide a simpler data container.
    /// 
    /// </summary>
    public class ReportContainer
    {
        public ReportContainer()
        {
            StudentReport = new List<StudentReport>();
            CreditReport = new List<CreditReport>();
            BudgetReport = new List<BudgetReport>();
            
            CategoryReport = new List<CategoryReport>();
            CourseReport = new List<CourseReport>();
            ProviderReport = new List<ProviderReport>();
            PrimaryReport = new List<PrimaryReport>();
            CourseFeeReport = new List<CourseFeeReport>();
        }

        public List<StudentReport> StudentReport { get; set; }
        public List<CreditReport> CreditReport { get; set; }
        public List<ProviderReport> ProviderReport { get; set; }
        public List<PrimaryReport> PrimaryReport { get; set; }
        public List<BudgetReport> BudgetReport { get; set; }
        
        public List<CategoryReport> CategoryReport { get; set; }
        public List<CourseReport> CourseReport { get; set; }
        public List<CourseFeeReport> CourseFeeReport { get; set; }
    }

    public class StudentReport
    {
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string SSID { get; set; }
    }

    public class CreditReport
    {
        public string Value { get; set; }
    }

    public class ProviderReport
    {
        public string Name { get; set; }
    }

    public class PrimaryReport
    {
        public string Name { get; set; }
        
    }

    public class BudgetReport
    {
        public decimal? BudgetPrimaryProvider { get; set; }
        public decimal? PriorDisbursementProvider{ get; set; }
        public decimal? TotalDisbursementsProvider { get; set; }
        public decimal? OffSet { get; set; }
        public decimal? Distribution { get; set; }
    }

    public  class CategoryReport
    {
        public string Name { get; set; }
    }

    public class CourseReport
    {
        public string Name { get; set; }
    }

    public class CourseFeeReport
    {
        public decimal Fee { get; set; }
    }
}