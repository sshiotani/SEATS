using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CcaRegistrationDf.DAL;
using CcaRegistrationDf.Models;
using Microsoft.AspNet.Identity;
using AutoMapper;

namespace CcaRegistrationDf.Controllers
{
    [Authorize]
    public class CCAsController : Controller
    {
        private readonly string[] FEDOPTIONS =  {
        "Allowed by College and Career Ready Plan (SEOP or CCRP) providing for Early Graduation",
        "Allowed by school district or charter school board policy (check with your school district office)" };

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CCAs
        public async Task<ActionResult> Index()
        {
            List<CCA> ccas;
            if (User.IsInRole("Admin"))
            {
                ccas = await db.CCAs.Include(c => c.Counselor).Include(c => c.OnlineCourse).Include(c => c.CourseCredit).Include(c => c.Primary).Include(c => c.Provider).Include(c => c.Student).ToListAsync();
                return View(ccas);
            }
            else
            {
                var UserIdentity = User.Identity.GetUserId();
                ccas = await db.CCAs.Where(m => m.UserId == UserIdentity).ToListAsync();
            }
            List<StudentStatusViewModel> courses = new List<StudentStatusViewModel>();
            Mapper.CreateMap<CCA, StudentStatusViewModel>();

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
        private async Task<StudentStatusViewModel> GetStatusReport(CCA cca)
        {
            var status = new StudentStatusViewModel();
            try
            {
                status.Session = await db.Session.Where(m => m.ID == cca.SessionID).FirstAsync().ConfigureAwait(false);
                status.Provider = await db.Providers.Where(m => m.ID == cca.ProviderID).FirstAsync().ConfigureAwait(false);
                status.OnlineCourse = await db.Courses.Where(m => m.ID == cca.OnlineCourseID).FirstAsync().ConfigureAwait(false);
                status.Category = await db.CourseCategories.Where(m => m.ID == cca.CourseCategoryID).FirstAsync().ConfigureAwait(false);
                status.ApplicationSubmissionDate = (DateTime)cca.ApplicationSubmissionDate;
                status.CompletionStatus = cca.CompletionStatus;

                return status;
            }
            catch
            {
                throw;
            }


        }
        // GET: CCAs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CCA cCA = await db.CCAs.FindAsync(id);
            if (cCA == null)
            {
                return HttpNotFound();
            }
            return View(cCA);
        }

        // GET: CCAs/Create
        public ActionResult Create()
        {
            var model = new CCAViewModel();
            var userId = User.Identity.GetUserId();
            var lea = db.Students.Where(m => m.UserId == userId).Select(m => m.EnrollmentLocationID).FirstOrDefault();
            model.EnrollmentLocationID = (int)lea;
            model = GetSelectLists(model);

            return View(model);
        }

        private CCAViewModel GetSelectLists(CCAViewModel model)
        {
            var lea = model.EnrollmentLocationID;
            if (lea == 0 || lea == 1)
            {
                model.CounselorList = new List<SelectListItem>();
            }
            else
            {
                var user = User.Identity.GetUserId();
                var schoolID = db.Students.Where(m => m.UserId == user).Select(m => m.EnrollmentLocationSchoolNamesID).FirstOrDefault();

                model.CounselorList = db.Counselors.Where(m => m.SchoolID == schoolID).Select(f => new SelectListItem
                {
                    Value = f.ID.ToString(),
                    Text = f.CounselorFirstName + " " + f.CounselorLastName
                });
            }

            model.ExcessiveFEDReasonList = db.ExcessiveFEDReasons.Select(f => new SelectListItem
            {
                Value = f.ID.ToString(),
                Text = f.Reason
            });

            model.Session = db.Session.Where(x => x.Name != "All" && x.IsActive).Select(f => new SelectListItem
            {
                Value = f.ID.ToString(),
                Text = f.Name
            });

            model.CourseCategory = new List<SelectListItem>();

            model.OnlineCourse = new List<SelectListItem>();

            model.CourseCredit = new List<SelectListItem>();

            return model;
        }

        // POST: CCAs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SubmitterTypeID,StudentGradeLevel,HasExcessiveFED,ExcessiveFEDExplanation,ExcessiveFEDReasonID,CounselorID,CounselorCactusID,CounselorEmail,CounselorFirstName,CounselorLastName,CounselorPhoneNumber,ProviderID,TeacherCactusID,TeacherFirstName,TeacherLastName,OnlineCourseID,CourseCategoryID,CourseCreditID,SessionID,Comments,EnrollmentLocationID")] CCAViewModel ccaVm)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<CCAViewModel, CCA>();

                CCA cca = Mapper.Map<CCAViewModel, CCA>(ccaVm);

                //If private school counselor is not found create a new one.
                if (ccaVm.EnrollmentLocationID == 2)
                {
                    int? counselorId = await db.Counselors.Where(x => x.CounselorEmail == ccaVm.CounselorEmail).Select(x => x.ID).FirstOrDefaultAsync();

                    
                    if (counselorId == null)
                    {
                        var counselor = new Counselor()
                        {
                            CounselorEmail = ccaVm.CounselorEmail,
                            CounselorFirstName = ccaVm.CounselorFirstName,
                            CounselorLastName = ccaVm.CounselorLastName,
                            CounselorPhoneNumber = ccaVm.CounselorPhoneNumber
                        };

                        db.Counselors.Add(counselor);

                        cca.CounselorID = db.Counselors.Last().ID;
                    }
                }

                cca.ApplicationSubmissionDate = DateTime.Now;
                cca.UserId = User.Identity.GetUserId();
                cca.StudentID = await db.Students.Where(x => x.UserId == cca.UserId).Select(x=> x.ID).FirstAsync();
                

                db.CCAs.Add(cca);
                await db.SaveChangesAsync();
                return View("Success");
            }

            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            foreach (var error in errors)
                ModelState.AddModelError("", error.Select(x => x.ErrorMessage).First());

            ccaVm = GetSelectLists(ccaVm);

            return View(ccaVm);

        }

        /// <summary>
        /// Gets Categories from database using session.
        /// Check courses that are associated with the session and only return categories
        /// that have courses in that session.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>select list in json format</returns>
        public async Task<JsonResult> GetCategories(int sessionId)
        {
            try
            {
                IEnumerable<int>  categorySelected;

                var courses = await db.Courses.ToListAsync().ConfigureAwait(false);

                var selected = await db.Session.Where(x => x.ID == sessionId).Select(x => x.Name).FirstOrDefaultAsync().ConfigureAwait(false);
                // If session is a summer session only show categories that have classes for summer
                if (selected.ToLower().Contains("sum"))
                {
                    categorySelected = courses.Where(x => x.SessionID == sessionId).Select(x => x.CourseCategoryID).Distinct();
                }
                else // Show all non summer categories
                {
                  categorySelected = courses.Where(x => x.SessionID == sessionId || x.SessionID == 0).Select(x => x.CourseCategoryID).Distinct();
                }

                var categoryList = db.CourseCategories.Where(y => categorySelected.Contains(y.ID) && y.IsActive ).Select(f => new SelectListItem
                    {
                        Value = f.ID.ToString(),
                        Text = f.Name
                    }); 

                return Json(new SelectList(categoryList, "Value", "Text"));
            }
            catch
            {
                throw new HttpException(500, "Error processing request.");
            }

        }

        /// <summary>
        /// Gets course name from database from choice from form using category id.  Course, provider,
        /// and Session must be active.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>select list in json format</returns>
        public async Task<JsonResult> GetCourseNames(int categoryId, int sessionId)
        {
            try
            {
                IEnumerable<SelectListItem> courseNameList;

                var courses = await db.Courses.ToListAsync().ConfigureAwait(false);

                var selected = await db.Session.Where(x => x.ID == sessionId).Select(x => x.Name).FirstOrDefaultAsync().ConfigureAwait(false);

                if (selected.ToLower().Contains("sum")) // Get summer courses
                {
                    courseNameList = courses.Where(x => x.CourseCategory.ID == categoryId && x.IsActive && x.Provider.IsActive && x.Session.IsActive && x.SessionID == sessionId).Select(f => new SelectListItem
                    {
                        Value = f.ID.ToString(),
                        Text = f.Name + " - " + f.Provider.Name
                    });
                }
                else // Get non summer courses
                {
                    courseNameList = courses.Where(x => x.CourseCategory.ID == categoryId && x.IsActive && x.Provider.IsActive && x.Session.IsActive && (x.SessionID == 0 || x.SessionID == sessionId) ).Select(f => new SelectListItem
                    {
                        Value = f.ID.ToString(),
                        Text = f.Name + " - " + f.Provider.Name
                    });
                }

                return Json(new SelectList(courseNameList, "Value", "Text"));
            }
            catch
            {
                throw new HttpException(500, "Error processing request.");
            }

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
                    Name = f.Provider.Name,
                    Credit = f.Credit,
                    OnlineProviderID = f.ProviderID,
                    Notes = f.Notes
                }).FirstOrDefault();

                courseResult.CreditChoices = await GetCourseCredit(courseResult.Credit);

                return (Json(courseResult));
            }
            catch
            {
                throw new HttpException(500, "Error processing request.");

            }
        }

        /// <summary>
        /// Gets course credit list from 4 char credit string in courses.  Populates list with credit options
        /// as follows; position 0: 0.25, postion 1: 0.50, position 2: 0.75, position 3: 1.00.
        /// So a string of "0100" would have a credit of 0.50 available for course
        /// and a string of "1111" would have credit options of 0.25, 0.50, 0.75, and 1.00.
        /// </summary>
        /// <param name="credits"></param>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> GetCourseCredit(string credits)
        {
            var creditOptions = await db.CourseCredits.ToListAsync();

            var creditList = creditOptions.Select(f => new SelectListItem
            {
                Value = f.ID.ToString(),
                Text = f.Value
            });

            var setList = creditList.ToList();

            if (credits != null)
            {
                char[] creditArray = credits.ToCharArray();

                for (int j = 0; j < creditOptions.Count(); j++)
                {

                    if (creditArray[j] == '0')
                        setList[j].Disabled = true;
                }
            }

            creditList = setList;

            return creditList;
        }

        // GET: CCAs/Edit/5
    
        [Authorize(Roles="Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CCA cCA = await db.CCAs.FindAsync(id);
            if (cCA == null)
            {
                return HttpNotFound();
            }
            ViewBag.CounselorID = new SelectList(db.Counselors, "ID", "CounselorEmail", cCA.CounselorID);
            ViewBag.CourseID = new SelectList(db.Courses, "ID", "Name", cCA.OnlineCourseID);
            ViewBag.CourseCreditID = new SelectList(db.CourseCredits, "ID", "ID", cCA.CourseCreditID);

            return View(cCA);
        }

        // POST: CCAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "SubmitterTypeID,StudentGradeLevel,HasExcessiveFED,ExcessiveFEDExplanation,ExcessiveFEDReasonID,CounselorID,IsCounselorSigned,ProviderID,CourseID,CourseCategoryID,CourseCreditID,SessionID,Comments")] CCAViewModel ccaVm)
        {
           
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<CCAViewModel, CCA>();

                CCA cca = Mapper.Map<CCAViewModel, CCA>(ccaVm);
                db.Entry(cca).State = EntityState.Modified;

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CounselorID = new SelectList(db.Counselors, "ID", "CounselorEmail", ccaVm.CounselorID);
            ViewBag.CourseID = new SelectList(db.Courses, "ID", "Name", ccaVm.OnlineCourseID);
            ViewBag.CourseCreditID = new SelectList(db.CourseCredits, "ID", "ID", ccaVm.CourseCreditID);

            return View(ccaVm);
        }

        // GET: CCAs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CCA cCA = await db.CCAs.FindAsync(id);
            if (cCA == null)
            {
                return HttpNotFound();
            }
            return View(cCA);
        }

        // POST: CCAs/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CCA cCA = await db.CCAs.FindAsync(id);
            db.CCAs.Remove(cCA);
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
