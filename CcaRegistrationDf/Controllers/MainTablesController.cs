using AutoMapper;
using CcaRegistrationDf.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CcaRegistrationDf.Controllers
{
    [Authorize]
    public class MainTablesController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        //TODO: Put these lookups in a table?
        private readonly string[] CREDITOPTIONS = { "0.25", "0.50", "0.75", "1.00" };
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
            var model = new MainFormViewModel();
            try
            {
                model = await GetClientSelectLists(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View("Error");
            }

            return View(model);
        }

        private async Task<MainFormViewModel> GetClientSelectLists(MainFormViewModel model)
        {
            try
            {
                //Look up Lists of Leas
                model.EnrollmentLocation = await GetEnrollmentLocations();

                //Look up Lists of Schools
                model.EnrollmentLocationSchoolNames = GetSchoolNames();

                // List of Excessive credit reasons
                model.ExcessiveFEDReasonList = GetFEDReasonList();

                //Get Semesters

                model.Semester = await GetSemesterList();

                //Look up List of Courses

                model.CourseCategory = await GetCourseCategories();
                model.CourseName = GetCourseNames();
                model.CourseCredit = GetCourseCredit();


                return model;
            }
            catch
            {
                throw;
            }
        }



        private List<SelectListItem> GetCourseCredit()
        {
            return new List<SelectListItem>();
        }

        private List<SelectListItem> GetCourseCredit(string credits)
        {

            SelectListItem listItem;

            var creditList = new List<SelectListItem>();

            if (credits != null)
            {
                char[] creditArray = credits.ToCharArray();

                for (int j = 0; j < CREDITOPTIONS.Count(); j++)
                {
                    listItem = new SelectListItem();
                    listItem.Value = (j + 1).ToString();
                    listItem.Text = CREDITOPTIONS[j];
                    if (creditArray[j] == '0')
                        listItem.Disabled = true;

                    creditList.Add(listItem);
                }
            }

            return creditList;
        }

        private async Task<IEnumerable<SelectListItem>> GetSemesterList()
        {
            try
            {
                var sessionList = await db.Sessions.ToListAsync();

                var session = sessionList.Where(x => x.IsActive && x.Name != "All").Select(f => new SelectListItem
                {
                    Value = f.ID.ToString(),
                    Text = f.Name
                });

                return session;
            }
            catch
            {
                throw;
            }
        }

        private List<SelectListItem> GetCourseNames()
        {
            return new List<SelectListItem>();
        }

        public async Task<JsonResult> GetCourseNames(int categoryId)
        {
            try
            {
                IEnumerable<SelectListItem> courseNameList;

                var courses = await db.Courses.ToListAsync();

                courseNameList = courses.Where(x => x.Category.ID == categoryId && x.IsActive && x.OnlineProvider.IsActive && x.Session.IsActive).Select(f => new SelectListItem
                {
                    Value = f.ID.ToString(),
                    Text = f.Name + " - " + f.OnlineProvider.Name
                });

                return Json(new SelectList(courseNameList, "Value", "Text"));
            }
            catch
            {
                throw new HttpException(500, "Error processing request.");
            }

        }

        private async Task<IEnumerable<SelectListItem>> GetCourseCategories()
        {
            try
            {
                var categoryList = await db.Categories.ToListAsync();

                var courseCategories = categoryList.Where(x => x.IsActive).Select(f => new SelectListItem
                {
                    Value = f.ID.ToString(),
                    Text = f.Name
                });


                return courseCategories;
            }
            catch
            {
                throw;
            }
        }

        private List<SelectListItem> GetFEDReasonList()
        {
            var fedList = new List<SelectListItem>();
            SelectListItem fedListItem;

            for (int i = 1; i <= FEDOPTIONS.Count(); i++)
            {
                fedListItem = new SelectListItem();
                fedListItem.Value = i.ToString();
                fedListItem.Text = FEDOPTIONS[i - 1];

                fedList.Add(fedListItem);
            }

            return fedList;
        }

        public async Task<JsonResult> GetCourseInformation(int courseId)
        {
            try
            {
                var courses = await db.Courses.ToListAsync();

                var courseResult = courses.Where(x => x.ID == courseId).Select(f => new CourseResultModel()
                {
                    Code = f.Code,
                    Name = f.OnlineProvider.Name,
                    Credit = f.Credit
                }).FirstOrDefault();

                courseResult.CreditChoices = GetCourseCredit(courseResult.Credit);

                return (Json(courseResult));
            }
            catch
            {
                throw new HttpException(500, "Error processing request.");

            }
        }

        private List<SelectListItem> GetSchoolNames()
        {
            return new List<SelectListItem>();
        }

        private async Task<IEnumerable<SelectListItem>> GetEnrollmentLocations()
        {
            var leaList = await db.EnrollmentLocations.ToListAsync();

            var locations = leaList.Select(f => new SelectListItem
                {
                    Value = f.ID.ToString(),
                    Text = f.Name
                });

            return locations;
        }

        // POST: MainTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SubmitterTypeID,StudentDOB,StudentEmail,StudentFirstName,StudentGradeLevel,StudentLastName,SSID,GraduationDate, EnrollmentLocationID, SchoolOfRecord, EnrollmentLocationSchoolNamesID,GuardianEmail,GuardianFirstName,GuardianLastName,GuardianPhone1,CounselorCactusID,CounselorEmail,CounselorFirstName,CounselorLastName,CounselorPhoneNumber,SemesterID,CourseCategoryID,CourseNameID,CourseCreditID,OnlineProviderID,IsCourseConsistentWithStudentSEOP,IsEarlyGraduate,IsFeeWaived,IsSection504,IsIEP, Comments,HasExcessiveFED, ExcessiveFEDExplanation,ExcessiveFEDReasonCode,TeacherCactusID,ProviderEmail,ProviderFax, ProviderFirstName,ProviderLastNameProviderPhoneNumber,TeacherFirstName,TeacherLastName")] MainFormViewModel mainFormViewModel)
        {


            if (ModelState.IsValid)
            {
                try
                {

                    Mapper.CreateMap<MainFormViewModel, MainTable>();

                    MainTable mainTable = Mapper.Map<MainFormViewModel, MainTable>(mainFormViewModel);
                    mainTable.ApplicationSubmissionDate = DateTime.Now;
                    mainTable.AspNetUserId = User.Identity.GetUserId();

                    db.MainTables.Add(mainTable);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View("Error");
                }
            }


            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            foreach (var error in errors)
                ModelState.AddModelError("", error.Select(x => x.ErrorMessage).First());

            //In order to maintain selectList Values we must call the GetClientSelectLists setup method

            return View(await GetClientSelectLists(mainFormViewModel));
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
