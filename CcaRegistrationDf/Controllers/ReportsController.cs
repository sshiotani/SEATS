using CcaRegistrationDf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using AutoMapper;

using Microsoft.AspNet.Identity;

using System.Data.Entity;

using System.Net;

using Microsoft.Reporting.WebForms;
using System.IO;
using System.Globalization;
using CcaRegistrationDf.Model;



namespace CcaRegistrationDf.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private SEATSEntities cactus = new SEATSEntities();

        // GET: Reports
        public async Task<ActionResult> Index()
        {
            List<ReportViewModel> reports = await GetReport().ConfigureAwait(false);

            return View(reports);
        }

        private async Task<List<ReportViewModel>> GetReport()
        {
            var ccas = await db.CCAs.ToListAsync().ConfigureAwait(false);
            
            List<ReportViewModel> report = new List<ReportViewModel>();

            foreach (var cca in ccas)
            {

                ReportViewModel line = new ReportViewModel();
                line.Id = cca.ID;

                line.StudentFirstName = cca.Student.StudentFirstName;
                line.StudentLastName = cca.Student.StudentLastName;
                line.SSID = cca.Student.SSID.ToString();


                line.Provider = await db.Providers.Where(m => m.ID == cca.ProviderID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);

                if (cca.EnrollmentLocationID == 1)
                    line.EnrollmentLocation = "HOME SCHOOL";
                else if (cca.EnrollmentLocationID == 2)
                    line.EnrollmentLocation = "PRIVATE SCHOOL";
                else
                    line.EnrollmentLocation = await cactus.CactusInstitutions.Where(m => m.ID == cca.EnrollmentLocationID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);

                line.CourseCredit = await db.CourseCredits.Where(m => m.ID == cca.CourseCreditID).Select(m => m.Value).FirstOrDefaultAsync().ConfigureAwait(false);
                line.BudgetPrimaryProvider = cca.BudgetPrimaryProvider;
                line.PriorDisbursementProvider = cca.PriorDisbursementProvider;
                line.TotalDisbursementsProvider = cca.TotalDisbursementsProvider;

               

                line.CourseCategory = cca.CourseCategory.Name;
                line.OnlineCourse = cca.OnlineCourse.Name;

                report.Add(line);

            }

            return report;
        }

        private async Task<ReportContainer> MakeReport()
        {
            var ccas = await db.CCAs.ToListAsync().ConfigureAwait(false);
            

            ReportContainer report = new ReportContainer();

            foreach (var cca in ccas)
            {
                var student = new StudentReport();
                student.StudentFirstName = cca.Student.StudentFirstName;
                student.StudentLastName = cca.Student.StudentLastName;
                student.SSID = cca.Student.SSID.ToString();
                report.StudentReport.Add(student);

                var primary = new PrimaryReport();
                if (cca.EnrollmentLocationID == 1)
                    primary.Name = "HOME SCHOOL";
                else if (cca.EnrollmentLocationID == 2)
                    primary.Name = "PRIVATE SCHOOL";
                else
                    primary.Name = await cactus.CactusInstitutions.Where(m => m.ID == cca.EnrollmentLocationID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);
                report.PrimaryReport.Add(primary);

                var provider = new ProviderReport();
                provider.Name = await db.Providers.Where(m => m.ID == cca.ProviderID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);
                report.ProviderReport.Add(provider);

                var credit = new CreditReport();
                credit.Value = await db.CourseCredits.Where(m => m.ID == cca.CourseCreditID).Select(m => m.Value).FirstOrDefaultAsync().ConfigureAwait(false);
                report.CreditReport.Add(credit);

                var budget = new BudgetReport();
                budget.BudgetPrimaryProvider = cca.BudgetPrimaryProvider;
                budget.PriorDisbursementProvider = cca.PriorDisbursementProvider;
                budget.TotalDisbursementsProvider = cca.TotalDisbursementsProvider;
                budget.OffSet = cca.Offset;
                budget.Distribution = cca.Distribution;
                report.BudgetReport.Add(budget);

                var category = new CategoryReport();
                var categoryItem =  await db.CourseCategories.Where(m => m.ID == cca.CourseCategoryID).FirstOrDefaultAsync().ConfigureAwait(false);
                category.Name = categoryItem.Name;
                report.CategoryReport.Add(category);

                var course = new CourseReport();
                course.Name = await db.Courses.Where(m => m.ID == cca.OnlineCourseID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);
                report.CourseReport.Add(course);

                var courseFee = new CourseFeeReport();
                courseFee.Fee = await db.CourseFees.Where(m => m.ID == categoryItem.CourseFeeID).Select(m => m.Fee).FirstOrDefaultAsync().ConfigureAwait(false);
                report.CourseFeeReport.Add(courseFee);

            }

            return report;
        }

        public async Task<ActionResult> GenerateReport(string id)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/ReportFormats"), "Report1.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                ViewBag.Message = "Report File does not exist.";
                return View("Error");
            }

            ReportContainer report = await MakeReport().ConfigureAwait(false);

            
            lr.DataSources.Add(new ReportDataSource("StudentData", report.StudentReport));
            lr.DataSources.Add(new ReportDataSource("ReportMonthly", report.BudgetReport));
            lr.DataSources.Add(new ReportDataSource("Credit", report.CreditReport));
            //lr.DataSources.Add(new ReportDataSource("StudentBudget", report.StudentBudgetReport));
            lr.DataSources.Add(new ReportDataSource("CourseInformation", report.CategoryReport));
            lr.DataSources.Add(new ReportDataSource("CourseName", report.CourseReport));
            lr.DataSources.Add(new ReportDataSource("EnrollmentLocations", report.PrimaryReport));
            lr.DataSources.Add(new ReportDataSource("Provider", report.ProviderReport));
            lr.DataSources.Add(new ReportDataSource("CourseFee", report.CourseFeeReport));

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }

    }
}