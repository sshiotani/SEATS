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
        /// <summary>
        /// Populates cca list for view.  Admin sees all ccas, users only see ccas associated with their
        /// UserID
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            List<MainTable> ccas;
            if(User.IsInRole("Admin"))
            {
                ccas = await db.MainTables.ToListAsync();
            }
            else
            {
                var UserIdentity = User.Identity.GetUserId();
                ccas = await db.MainTables.Where(m => m.UserId == UserIdentity).ToListAsync();
            }

            List<CourseStatusViewModel> courses = new List<CourseStatusViewModel>();
            Mapper.CreateMap<MainTable, CourseStatusViewModel>();

            foreach (var cca in ccas)
            {
                try
                {
                    var course = await GetStatusReport(cca);
                    courses.Add(course);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(courses);
        }

        /// <summary>
        /// Generates view model that populates names from IDs in cca database.
        /// </summary>
        /// <param name="cca"></param>
        /// <returns></returns>
        private async Task<CourseStatusViewModel> GetStatusReport(MainTable cca)
        {
            var status = new CourseStatusViewModel();
            try
            {
                status.Session = await db.Sessions.Where(m => m.ID == cca.SessionID).FirstAsync().ConfigureAwait(false);
                status.OnlineProvider = await db.OnlineProviders.Where(m => m.ID == cca.OnlineProviderID).FirstAsync().ConfigureAwait(false);
                status.Course = await db.Courses.Where(m => m.ID == cca.CourseID).FirstAsync().ConfigureAwait(false);
                status.Category = await db.Categories.Where(m => m.ID == cca.CategoryID).FirstAsync().ConfigureAwait(false);
                status.ApplicationSubmissionDate = (DateTime)cca.ApplicationSubmissionDate;
                status.CompletionStatus = cca.CompletionStatus;

                return status;
            }
            catch
            {
                throw;
            }


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

        // POST: MainTables/Create
        /// <summary>
        /// Populates the view model from the form.
        /// </summary>
        /// <param name="mainFormViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SubmitterTypeID,StudentDOB,StudentEmail,StudentFirstName,StudentGradeLevel,StudentLastName,SSID,GraduationDate, EnrollmentLocationID, SchoolOfRecord, EnrollmentLocationSchoolNamesID,GuardianEmail,GuardianFirstName,GuardianLastName,GuardianPhone1,CounselorCactusID,CounselorEmail,CounselorFirstName,CounselorLastName,CounselorPhoneNumber,SessionID,CategoryID,CourseID,CreditID,OnlineProviderID,IsCourseConsistentWithStudentSEOP,IsEarlyGraduate,IsFeeWaived,IsSection504,IsIEP, Comments,HasExcessiveFED, ExcessiveFEDExplanation,ExcessiveFEDReasonCode,TeacherCactusID,ProviderEmail,ProviderFax, ProviderFirstName,ProviderLastNameProviderPhoneNumber,TeacherFirstName,TeacherLastName")] MainFormViewModel mainFormViewModel)
        {


            if (ModelState.IsValid)
            {
                try
                {

                    Mapper.CreateMap<MainFormViewModel, MainTable>();

                    MainTable mainTable = Mapper.Map<MainFormViewModel, MainTable>(mainFormViewModel);
                    mainTable.ApplicationSubmissionDate = DateTime.Now;
                    mainTable.UserId = User.Identity.GetUserId();

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

            mainFormViewModel.CategoryID = 0;  // Reset choices so student has to choose and populate course choice.


            return View(await GetClientSelectLists(mainFormViewModel));
        }

       
        /// <summary>
        /// Helper methods that populate select lists.  Some are dynamically populated using 
        /// selected items.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        #region ListHelpers
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

                model.Session = await GetSemesterList();

                //Look up List of Courses

                model.Category = await GetCourseCategories();
                model.Course = GetCourseNames();
                model.Credit = GetCourseCredit();


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

        /// <summary>
        /// Gets course credit list from 4 char credit string in courses.  Populates list with credit options
        /// as follows; position 0: 0.25, postion 1: 0.50, position 2: 0.75, position 3: 1.00.
        /// So a string of "0100" would have a credit of 0.50 available for course
        /// and a string of "1111" would have credit options of 0.25, 0.50, 0.75, and 1.00.
        /// </summary>
        /// <param name="credits"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get semester (sessions) from database.  Only populate list from active items.  Filters out the 
        /// All option.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets course name from database from choice from form using category id.  Course, provider,
        /// and Session must be active.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>select list in json format</returns>
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

        /// <summary>
        /// Get list of course categories from database for list population.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets list of reasons for list for more than full day equivalent credit.  Currently set from
        /// a readonly string at top of controller.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get course information from course chosen in dropdown list from form.
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetCourseInformation(int courseId)
        {
            try
            {
                var courses = await db.Courses.ToListAsync();

                var courseResult = courses.Where(x => x.ID == courseId).Select(f => new CourseResultModel()
                {
                    Code = f.Code,
                    Name = f.OnlineProvider.Name,
                    Credit = f.Credit,
                    OnlineProviderID = f.OnlineProviderID
                }).FirstOrDefault();

                courseResult.CreditChoices = GetCourseCredit(courseResult.Credit);

                return (Json(courseResult));
            }
            catch
            {
                throw new HttpException(500, "Error processing request.");

            }
        }

        
        /// <summary>
        /// Populates Leas into select list. Inserts Home and Private at top.
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> GetEnrollmentLocations()
        {
            try
            {
                using (CactusEntities leas = new CactusEntities())
                {
                    var leaList = await leas.CactusInstitutions.OrderBy(m => m.Name).ToListAsync();

                    leaList.Insert(0, new CactusInstitution() { Name = "HOME SCHOOL", DistrictID = 1.0M });
                    leaList.Insert(0, new CactusInstitution() { Name = "PRIVATE SCHOOL", DistrictID = 2.0M });

                    var locations = leaList.Select(f => new SelectListItem
                        {
                            Value = f.DistrictID.ToString(),
                            Text = f.Name
                        });

                    return locations;
                }
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

        /// <summary>
        /// Method gets school names dynamically called from view. Matches the district id with
        /// the selected district id.  Removes null and list entries with DISTRICT selections.
        /// </summary>
        /// <param name="district"></param>
        /// <returns>json list with selectlist</returns>
        public async Task<JsonResult> GetSchoolNames(decimal district)
        {
            try
            {
                using (CactusEntities schools = new CactusEntities())
                {
                    IEnumerable<SelectListItem> schoolNameList;
                    var schoolList = await schools.CactusSchools.ToListAsync();
                    schoolList.RemoveAll(m => m.name == null);
                    schoolNameList = schoolList.Where(m => m.district == district && !m.name.Contains("DISTRICT")).OrderBy(m => m.name).Distinct().Select(f => new SelectListItem
                    {
                        Value = f.id.ToString(),
                        Text = f.name
                    });

                    return Json(new SelectList(schoolNameList, "Value", "Text"));
                }
            }
            catch
            {
                throw new HttpException(500, "Error processing request.");

            }
            
        }

#endregion

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
        public async Task<ActionResult> Edit([Bind(Include = "AgreementID,ApplicationSubmissionDate,AssessedProficiency,BudgetPrimaryProvider,Business_Administrator_Email,BusinessAdministratorFirstName,BusinessAdministratorLastName,BusinessAdministratorSignature,BusinessAdministratorTelephone,Comments,CompletionStatus,CounselorCactusID,CounselorEmail,CounselorFirstName,CounselorLastName,CounselorPhoneNumber,CourseBegin,CourseCompletionDate,CreditID,CourseFee,CourseID,CourseRecord2ndSemesterID,CourseRecordID,CourseStartDate,CreditCompletedToDate,DateBusinessAdministratorSignature,DateConfirmationActiveParticipation,DateContinuationActiveParticipation,DateReportPassingGrade,EnrollmentLocationID,ExcessiveFEDExplanation,ExcessiveFEDReasonCode,GraduationDate,GuardianEmail,GuardianFirstName,GuardianLastName,GuardianPhone1,GuardianPhone2,IsBusinessAdministratorAcceptRejectEnrollment,IsCounselorSigned,IsCourseConsistentWithStudentSEOP,IsEarlyGraduate,IsEnrollmentNoticeSent,IsFeeWaived,IsGuardianSigned,IsIEP,IsPrimaryEnrollmentVerified,IsProviderAcceptsRejectsCourseRequest,IsProviderEnrollmentVerified,IsProviderSignature,IsQ1,IsQ2,IsQ3,IsQ4,IsRemediation,IsSection504,IsStudentSigned,NotificationDate,NovemberFY15Distr,NovemberFY15Offset,OnlineProviderID,ParentTelephone2,PricingTier,PrimaryLEAExplantionRejection,PrimaryLEAReasonRejectingCCA,PrimaryNotificationDate,PriorDisbursementProvider,ProviderEmail,ProviderExplanationRejection,ProviderFax,ProviderFirstName,ProviderLastName,ProviderPhoneNumber,ProviderReasonRejection,RecordNotes,RemediationPeriodBegins,SchoolOfRecord,SSID,StartDateSecondSemester,StudentDOB,StudentEmail,StudentFirstName,StudentGradeLevel,StudentLastName,SubmissionDate,SubmitterTypeID,TeacherCactusID,TotalDisbursementsProvider,TwentyDaysPastSemesterStartDate,Unallocated,UnallocatedReduction,WithdrawalDate,Grand_Total")] MainTable mainTable)
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
