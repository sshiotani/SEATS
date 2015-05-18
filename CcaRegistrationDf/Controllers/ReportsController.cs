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
using CcaRegistrationDf.DAL;
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

        // GET: Reports
        public async Task<ActionResult> Index()
        {
            List<ReportViewModel> reports = await GetReport();

            return View(reports);
        }

        private async Task<List<ReportViewModel>> GetReport()
        {
            var ccas = await db.CCAs.ToListAsync();
            CactusEntities cactus = new CactusEntities();
            List<ReportViewModel> report = new List<ReportViewModel>();

            foreach (var cca in ccas)
            {

                ReportViewModel line = new ReportViewModel();

                line.StudentFirstName = cca.Student.StudentFirstName;
                line.StudentLastName = cca.Student.StudentLastName;
                line.SSID = cca.Student.SSID.ToString();


                line.Provider = await db.Providers.Where(m => m.ID == cca.ProviderID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);

                if (cca.EnrollmentLocationID == 1)
                    line.EnrollmentLocation = "HOME SCHOOL";
                else if (cca.EnrollmentLocationID == 2)
                    line.EnrollmentLocation = "PRIVATE SCHOOL";
                else
                    line.EnrollmentLocation = await cactus.CactusInstitutions.Where(m => m.DistrictID == cca.EnrollmentLocationID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);

                line.CourseCredit = await db.CourseCredits.Where(m => m.ID == cca.CourseCreditID).Select(m => m.Value).FirstOrDefaultAsync().ConfigureAwait(false);
                line.BudgetPrimaryProvider = cca.BudgetPrimaryProvider;
                line.PriorDisbursementProvider = cca.PriorDisbursementProvider;
                line.TotalDisbursementsProvider = cca.TotalDisbursementsProvider;

                int? studentBudgetId = await db.Students.Where(m => m.ID == cca.StudentID).Select(m => m.StudentBudgetID).FirstOrDefaultAsync();
                if (studentBudgetId != null)
                {
                    var now = DateTime.Now;
                    var month = now.Month;
                    var year = now.Year;

                    var studentBudget = db.StudentBudgets.Where(m => m.ID == studentBudgetId && m.Month == month && m.Year == year).FirstOrDefault();
                    if (studentBudget != null)
                    {
                        line.Offset = studentBudget.OffSet;
                        line.Distribution = studentBudget.Distribution;
                    }
                }

                line.CourseCategory = cca.CourseCategory.Name;
                line.OnlineCourse = cca.OnlineCourse.Name;

                report.Add(line);

            }



            //report.Credit = await db.CourseCredits.Where(m => m.ID == cca.CourseCreditID).FirstAsync();

            //report.Name = await db.CourseCategories.Where(m => m.ID == cca.CourseCategoryID).FirstAsync();
            //report.Name = await db.Courses.Where(m => m.ID == cca.OnlineCourseID).FirstAsync();
            //report.Provider = await db.Providers.Where(m => m.ID == cca.ProviderID).FirstAsync();

            //report.BudgetPrimaryProvider = cca.BudgetPrimaryProvider;
            //report.PriorDisbursementProvider = cca.PriorDisbursementProvider;
            //report.TotalDisbursementsProvider = cca.TotalDisbursementsProvider;



            return report;
        }

        private async Task<ReportContainer> MakeReport()
        {
            var ccas = await db.CCAs.ToListAsync();
            CactusEntities cactus = new CactusEntities();

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
                    primary.Name = await cactus.CactusInstitutions.Where(m => m.DistrictID == cca.EnrollmentLocationID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);
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
                report.BudgetReport.Add(budget);

                var studentBr = new StudentBudgetReport();

                int? studentBudgetId = await db.Students.Where(m => m.ID == cca.StudentID).Select(m => m.StudentBudgetID).FirstOrDefaultAsync().ConfigureAwait(false);
                if (studentBudgetId != null)
                {
                    var now = DateTime.Now;
                    var month = now.Month;
                    var year = now.Year;

                    var studentBudget = await db.StudentBudgets.Where(m => m.ID == studentBudgetId && m.Month == month && m.Year == year).FirstOrDefaultAsync().ConfigureAwait(false);
                    if (studentBudget != null)
                    {
                        studentBr.OffSet = studentBudget.OffSet;
                        studentBr.Distribution = studentBudget.Distribution;
                    }

                }
                report.StudentBudgetReport.Add(studentBr);

                var category = new CategoryReport();
                category.Name = await db.CourseCategories.Where(m => m.ID == cca.CourseCategoryID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);
                report.CategoryReport.Add(category);

                var course = new CourseReport();
                course.Name = await db.Courses.Where(m => m.ID == cca.OnlineCourseID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);
                report.CourseReport.Add(course);

            }

            return report;
        }

        public async Task<ActionResult> GenerateReport(string id)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "Report1.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                ViewBag.Message = "Report File does not exist.";
                return View("Error");
            }

            ReportContainer report = await MakeReport();

            ReportDataSource rd1 = new ReportDataSource("StudentData", report.StudentReport);
            lr.DataSources.Add(rd1);
            ReportDataSource rd2 = new ReportDataSource("ReportMonthly", report.BudgetReport);
            lr.DataSources.Add(rd2);
            ReportDataSource rd3 = new ReportDataSource("Credit", report.CreditReport);
            lr.DataSources.Add(rd3);
            ReportDataSource rd4 = new ReportDataSource("StudentBudget", report.StudentBudgetReport);
            lr.DataSources.Add(rd4);
            ReportDataSource rd5 = new ReportDataSource("CourseInformation", report.CategoryReport);
            lr.DataSources.Add(rd5);
            ReportDataSource rd6 = new ReportDataSource("CourseName", report.CourseReport);
            lr.DataSources.Add(rd6);
            ReportDataSource rd7 = new ReportDataSource("EnrollmentLocations", report.PrimaryReport);
            lr.DataSources.Add(rd7);
            ReportDataSource rd8 = new ReportDataSource("Provider", report.ProviderReport);
            lr.DataSources.Add(rd8);

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