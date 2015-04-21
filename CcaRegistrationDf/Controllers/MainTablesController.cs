using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CcaRegistrationDf.Models;
using AutoMapper;

namespace CcaRegistrationDf.Controllers
{
    [Authorize]
    public class MainTablesController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        //TODO: Put these lookups in a table?
        private readonly string[] CREDITOPTIONS = {"0.25","0.50","0.75","1.00"};
        private readonly string[] FEDOPTIONS =  {
        "Allowed by College and Career Ready Plan (SEOP or CCRP) providing for Early Graduation",
        "Allowed by school district or charter school board policy (check with your school district office)" };

        // GET: MainTables
        public async Task<ActionResult> Index()
        {
            return View(await db.MainTables.ToListAsync());
        }

        // GET: MainTables/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainTable mainTable = await db.MainTables.FindAsync(id);
            if (mainTable == null)
            {
                return HttpNotFound();
            }
            return View(mainTable);
        }

        // GET: MainTables/Create
        public async Task<ActionResult> Create()
        {
            var model = new MainFormViewModel()
            {
                //Look up Lists of Leas
                EnrollmentLocation = await GetEnrollmentLocations(),

                //Look up Lists of Schools
                EnrollmentLocationSchoolNames = GetSchoolNames(),

                // List of Excessive credit reasons
                ExcessiveFEDReasonList = GetFEDReasonList(),

                //Get Semesters

                Semester = await GetSemesterList(),

                //Look up List of Courses

                CourseCategory = await GetCourseCategories(),
                CourseName = GetCourseNames(),
                CourseCredit = GetCourseCredit()

            };

            return View(model);
        }

        private List<SelectListItem> GetCourseCredit()
        {
            return new List<SelectListItem>();
        }

        private List<SelectListItem> GetCourseCredit(string credits)
        {
           
            SelectListItem listItem;
            var creditList = new List<SelectListItem>();

            char[] creditArray = credits.ToCharArray();

            for (int j = 0 ; j < CREDITOPTIONS.Count() ; j++)
            {
                listItem = new SelectListItem();
                listItem.Value = (j + 1).ToString();
                listItem.Text = CREDITOPTIONS[j];
                if (creditArray[j] == '0')
                    listItem.Disabled = true;

                creditList.Add(listItem);
            }

            return creditList;
        }

        private async Task<List<SelectListItem>> GetSemesterList()
        {

            var session = new List<SelectListItem>();

            var sessionList = await db.Sessions.ToListAsync();

            foreach (var sessionName in sessionList)
            {
                if (sessionName.IsActive)
                {
                    var sessionListItem = new SelectListItem();

                    sessionListItem.Text = sessionName.Name;
                    sessionListItem.Value = sessionName.ID.ToString();

                    session.Add(sessionListItem);
                }
            }


            return session;
        }



        private List<SelectListItem> GetCourseNames()
        {
            return new List<SelectListItem>();
        }

        public async Task<JsonResult> GetCourseNames(int categoryId)
        {

            var courseNameList = new List<SelectListItem>();

            var courses = await db.Courses.ToListAsync();


            foreach (var courseName in courses)
            {
                    if (courseName.Category.ID == categoryId && courseName.IsActive && courseName.OnlineProvider.IsActive && courseName.Session.IsActive)
                    {
                        var courseNameListItem = new SelectListItem();

                        courseNameListItem.Text = courseName.Name + " - " + courseName.OnlineProvider.Name;
                        courseNameListItem.Value = courseName.ID.ToString();

                        courseNameList.Add(courseNameListItem);
                    }
            }


            return Json(new SelectList(courseNameList, "Value", "Text"));
        }

        private async Task<List<SelectListItem>> GetCourseCategories()
        {
            var courseCategories = new List<SelectListItem>();
            SelectListItem categoryListItem;
            var categoryList = await db.Categories.ToListAsync();

            foreach (var category in categoryList)
            {
                if (category.IsActive)
                {
                    categoryListItem = new SelectListItem();

                    categoryListItem.Text = category.Name;
                    categoryListItem.Value = category.ID.ToString();

                    courseCategories.Add(categoryListItem);
                }
            }


            return courseCategories;
        }

        private List<SelectListItem> GetFEDReasonList()
        {
            var fedList = new List<SelectListItem>();
            SelectListItem fedListItem;

            for (int i=1 ; i <= FEDOPTIONS.Count() ;  i++)
            {
                fedListItem = new SelectListItem();
                fedListItem.Value = i.ToString();
                fedListItem.Text = FEDOPTIONS[i-1];

                fedList.Add(fedListItem);
            }

            return fedList;
        }

       

        public async Task<JsonResult> GetCourseInformation(int courseId)
        {
            var courseResult = new CourseResultModel();
            var courses = await db.Courses.ToListAsync();

            foreach (var courseName in courses)
            {

                if (courseName.ID == courseId)
                {
                    courseResult.Name = courseName.OnlineProvider.Name;
                    courseResult.Credit = courseName.Credit;
                    courseResult.Code = courseName.Code;
                }
            }

            courseResult.CreditChoices = GetCourseCredit(courseResult.Credit);


            return (Json(courseResult));
        }

        private List<SelectListItem> GetSchoolNames()
        {
            return new List<SelectListItem>();
        }

        private async Task<List<SelectListItem>> GetEnrollmentLocations()
        {
            var locations = new List<SelectListItem>();

            var leaList = await db.EnrollmentLocations.ToListAsync();

            foreach (var lea in leaList)
            {
                var leaListItem = new SelectListItem();

                leaListItem.Text = lea.Name;
                leaListItem.Value = lea.ID.ToString();

                locations.Add(leaListItem);
            }


            return locations;
        }

        // POST: MainTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MainFormViewModel mainFormViewModel)
        {


            if (ModelState.IsValid)
            {
                Mapper.CreateMap<MainFormViewModel, MainTable>();

                MainTable mainTable = Mapper.Map<MainFormViewModel, MainTable>(mainFormViewModel);
                mainTable.ApplicationSubmissionDate = DateTime.Now;

                db.MainTables.Add(mainTable);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(mainFormViewModel);
        }

        // GET: MainTables/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainTable mainTable = await db.MainTables.FindAsync(id);
            if (mainTable == null)
            {
                return HttpNotFound();
            }
            return View(mainTable);
        }

        // POST: MainTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AgreementID,ApplicationSubmissionDate,AssessedProficiency,BudgetPrimaryProvider,Business_Administrator_Email,BusinessAdministratorFirstName,BusinessAdministratorLastName,BusinessAdministratorSignature,BusinessAdministratorTelephone,Comments,CompletionStatus,CounselorCactusID,CounselorEmail,CounselorFirstName,CounselorLastName,CounselorPhoneNumber,CourseBegin,CourseCompletionDate,CourseCreditID,CourseFee,CourseID,CourseRecord2ndSemesterID,CourseRecordID,CourseStartDate,CreditCompletedToDate,DateBusinessAdministratorSignature,DateConfirmationActiveParticipation,DateContinuationActiveParticipation,DateReportPassingGrade,EnrollmentLocationID,ExcessiveFEDExplanation,ExcessiveFEDReasonCode,GraduationDate,GuardianEmail,GuardianFirstName,GuardianLastName,GuardianPhone1,GuardianPhone2,IsBusinessAdministratorAcceptRejectEnrollment,IsCounselorSigned,IsCourseConsistentWithStudentSEOP,IsEarlyGraduate,IsEnrollmentNoticeSent,IsFeeWaived,IsGuardianSigned,IsIEP,IsPrimaryEnrollmentVerified,IsProviderAcceptsRejectsCourseRequest,IsProviderEnrollmentVerified,IsProviderSignature,IsQ1,IsQ2,IsQ3,IsQ4,IsRemediation,IsSection504,IsStudentSigned,NotificationDate,NovemberFY15Distr,NovemberFY15Offset,OnlineProviderID,ParentTelephone2,PricingTier,PrimaryLEAExplantionRejection,PrimaryLEAReasonRejectingCCA,PrimaryNotificationDate,PriorDisbursementProvider,ProviderEmail,ProviderExplanationRejection,ProviderFax,ProviderFirstName,ProviderLastName,ProviderPhoneNumber,ProviderReasonRejection,RecordNotes,RemediationPeriodBegins,SchoolOfRecord,SSID,StartDateSecondSemester,StudentDOB,StudentEmail,StudentFirstName,StudentGradeLevel,StudentLastName,SubmissionDate,SubmitterTypeID,TeacherCactusID,TotalDisbursementsProvider,TwentyDaysPastSemesterStartDate,Unallocated,UnallocatedReduction,WithdrawalDate,Grand_Total")] MainTable mainTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mainTable).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(mainTable);
        }

        // GET: MainTables/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainTable mainTable = await db.MainTables.FindAsync(id);
            if (mainTable == null)
            {
                return HttpNotFound();
            }
            return View(mainTable);
        }

        // POST: MainTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MainTable mainTable = await db.MainTables.FindAsync(id);
            db.MainTables.Remove(mainTable);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
