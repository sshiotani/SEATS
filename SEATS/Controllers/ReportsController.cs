using SEATS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;
using OfficeOpenXml;
using System.Data;
using System;
using System.Drawing;
using OfficeOpenXml.Style;



namespace SEATS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private ApplicationDbContext db;
        private SEATSEntities cactus;


        //private SeatsContext db { get; set; }

        public ReportsController()
        {
            this.db = new ApplicationDbContext();
            this.cactus = new SEATSEntities();
        }


        // GET: Reports
        public async Task<ActionResult> Index()
        {
            List<ReportViewModel> reports = await DisplayReport().ConfigureAwait(false);

            return View(reports);
        }

        private async Task<List<ReportViewModel>> DisplayReport()
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

                if (cca.EnrollmentLocationID == GlobalVariables.HOMESCHOOLID)
                    line.EnrollmentLocation = "HOME SCHOOL";
                else if (cca.EnrollmentLocationID == GlobalVariables.PRIVATESCHOOLID)
                    line.EnrollmentLocation = "PRIVATE SCHOOL";
                else
                    line.EnrollmentLocation = await cactus.CactusInstitutions.Where(m => m.ID == cca.EnrollmentLocationID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);

                line.CourseCredit = await db.CourseCredits.Where(m => m.ID == cca.CourseCreditID).Select(m => m.Value).FirstOrDefaultAsync().ConfigureAwait(false);
                line.BudgetPrimaryProvider = cca.BudgetPrimaryProvider;
                line.PriorDisbursementProvider = cca.PriorDisbursementProvider;
                line.TotalDisbursementsProvider = cca.TotalDisbursementsProvider;



                line.CourseCategory = cca.CourseCategory.Name;
                line.OnlineCourse = cca.OnlineCourse.Name;
                line.CourseCompletionStatus = cca.CourseCompletionStatus;
                line.IsBusinessAdministratorAcceptRejectEnrollment = cca.IsBusinessAdministratorAcceptRejectEnrollment;
                line.CourseStartDate = cca.CourseStartDate;


                report.Add(line);

            }

            return report;
        }

        /// <summary>
        /// Makes a DataTable for use in the Excel generation method.
        /// </summary>
        /// <returns></returns>
        public ActionResult MakeDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Submission Date", typeof(string));
            table.Columns.Add("First Name", typeof(string));
            table.Columns.Add("Last Name", typeof(string));
            table.Columns.Add("SSID", typeof(string));
            table.Columns.Add("Credit", typeof(string));
            table.Columns.Add("Primary", typeof(string));
            table.Columns.Add("Primary Rejection Reason", typeof(string));
            table.Columns.Add("Provider", typeof(string));
            table.Columns.Add("Provider Rejection Reason", typeof(string));
            table.Columns.Add("Course Fee", typeof(string));
            table.Columns.Add("Budget", typeof(decimal));
            table.Columns.Add("Prior", typeof(decimal));
            table.Columns.Add("Total", typeof(decimal));
            table.Columns.Add("Offset", typeof(decimal));
            table.Columns.Add("Distribution", typeof(string));
            table.Columns.Add("Category", typeof(string));
            table.Columns.Add("Course", typeof(string));
            table.Columns.Add("Counselor Email", typeof(string));
            table.Columns.Add("Parent Email", typeof(string));
            table.Columns.Add("Start Date", typeof(string));

            var ccas = db.CCAs.ToList();

            foreach (var cca in ccas)
            {

                string primaryName;
                if (cca.EnrollmentLocationID == GlobalVariables.HOMESCHOOLID)
                    primaryName = "HOME SCHOOL";
                else if (cca.EnrollmentLocationID == GlobalVariables.PRIVATESCHOOLID)
                    primaryName = "PRIVATE SCHOOL";
                else
                    primaryName = cactus.CactusInstitutions.Where(m => m.ID == cca.EnrollmentLocationID).Select(m => m.Name).FirstOrDefault();

                string primaryRejectionReason = "";
                if (cca.PrimaryRejectionReasons != null)
                    primaryRejectionReason = cca.PrimaryRejectionReasons.Reason;

                string providerRejectionReason = "";
                if (cca.ProviderRejectionReasons != null)
                    providerRejectionReason = cca.ProviderRejectionReasons.Reason;






                table.Rows.Add(String.Format("{0:MM/dd/yyyy}", cca.ApplicationSubmissionDate),cca.Student.StudentFirstName, cca.Student.StudentLastName, cca.Student.SSID, cca.CourseCredit.Value, primaryName, primaryRejectionReason, cca.Provider.Name, providerRejectionReason, cca.CourseFee, cca.BudgetPrimaryProvider, cca.PriorDisbursementProvider, cca.TotalDisbursementsProvider, cca.Offset, cca.Distribution, cca.CourseCategory.Name, cca.OnlineCourse.Name, cca.Student.Parent.GuardianEmail, cca.Counselor.Email, cca.CourseStartDate);
            }

            TempData["Table"] = table;

            return RedirectToAction("DumpExcel");
        }


        /// <summary>
        /// Uses the EPPlus package to generate Excel spreadsheet from DataTable.
        /// </summary>
        public void DumpExcel()
        {
            var table = TempData["Table"] as DataTable;

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Monthly");
                sheet.Cells["A1"].LoadFromDataTable(table, true);

                // Fit Column to cell width
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                //Format the header
                using (ExcelRange rng = sheet.Cells["A1:Q1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                    rng.Style.Font.Color.SetColor(Color.White);
                }

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=Monthly-" + String.Format("{0:MM-dd-yyyy}", DateTime.Now) + ".xlsx");
                Response.BinaryWrite(package.GetAsByteArray());

            }
        }



    }
}