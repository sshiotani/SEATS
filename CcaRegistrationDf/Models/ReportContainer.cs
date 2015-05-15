using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Model
{
    public class ReportContainer
    {
        public ReportContainer()
        {
            StudentReport = new List<StudentReport>();
            CreditReport = new List<CreditReport>();
            BudgetReport = new List<BudgetReport>();
            StudentBudgetReport = new List<StudentBudgetReport>();
            CategoryReport = new List<CategoryReport>();
            CourseReport = new List<CourseReport>();
            ProviderReport = new List<ProviderReport>();
            PrimaryReport = new List<PrimaryReport>();

        }

        public List<StudentReport> StudentReport { get; set; }
        public List<CreditReport> CreditReport { get; set; }
        public List<ProviderReport> ProviderReport { get; set; }
        public List<PrimaryReport> PrimaryReport { get; set; }
        public List<BudgetReport> BudgetReport { get; set; }
        public List<StudentBudgetReport> StudentBudgetReport { get; set; }
        public List<CategoryReport> CategoryReport { get; set; }
        public List<CourseReport> CourseReport { get; set; }
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
    }

    public class StudentBudgetReport
    {
        public decimal? OffSet { get; set; }
        public decimal? Distribution { get; set; }
    }

    public  class CategoryReport
    {
        public string CourseCategory { get; set; }
    }

    public class CourseReport
    {
        public string OnlineCourse { get; set; }
    }

}