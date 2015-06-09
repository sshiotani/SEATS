﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;

using CcaRegistrationDf.Models;
using AutoMapper;

using Microsoft.AspNet.Identity.Owin;

namespace CcaRegistrationDf.Controllers
{
    [Authorize]
    public class CCAsController : Controller
    {


        private ApplicationDbContext db = new ApplicationDbContext();
        private SEATSEntities cactus = new SEATSEntities();

        // GET: CCAs
        /// <summary>
        /// Redirects User to the correct CCA interface according to their role.  Otherwise it creates a view designed for the 
        /// student user.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("CcaInterface", "Admin");
            }
            else if (User.IsInRole("Provider"))
            {
                return RedirectToAction("CcaInterface", "ProviderUsers");
            }
            else if (User.IsInRole("Primary"))
            {
                return RedirectToAction("CcaInterface", "PrimaryUsers");
            }
            else if (User.IsInRole("Counselor"))
            {
                return RedirectToAction("CcaInterface", "Counselors");
            }
            else
            {
                return RedirectToAction("StudentCcaView");
            }


        }

        /// <summary>
        /// Creates a list of CCAs designed for student viewing.  
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> StudentCcaView()
        {
            var UserIdentity = User.Identity.GetUserId();
            var ccas = await db.CCAs.Where(m => m.UserId == UserIdentity).ToListAsync().ConfigureAwait(false);

            List<StudentStatusViewModel> courses = new List<StudentStatusViewModel>();
            Mapper.CreateMap<CCA, StudentStatusViewModel>();

            foreach (var cca in ccas)
            {
                try
                {
                    var course = Mapper.Map<CCA, StudentStatusViewModel>(cca);
                    courses.Add(course);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(courses);
        }


        // GET: CCAs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                CCA cCA = await db.CCAs.FindAsync(id).ConfigureAwait(false);
                if (cCA == null)
                {
                    return HttpNotFound();
                }
                return View(cCA);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error retrieving details! Error: " + ex.Message;
                return View("Error");
            }
        }

        // GET: CCAs/Create
        public ActionResult Create()
        {
            try
            {
                var model = new CCAViewModel();
                var userId = User.Identity.GetUserId();
                var lea = db.Students.Where(m => m.UserId == userId).Select(m => m.EnrollmentLocationID).FirstOrDefault();
                model.EnrollmentLocationID = (int)lea;
                model = GetSelectLists(model);

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error getting Select List information! Error: " + ex.Message;
                return View("Error");
            }
        }

        /// <summary>
        /// Helpers to populate lists for Creating CCAs.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private CCAViewModel GetSelectLists(CCAViewModel model)
        {
            try
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

                    model.CounselorList = db.Counselors.Where(m => m.EnrollmentLocationSchoolNameID == schoolID).Select(f => new SelectListItem
                    {
                        Value = f.ID.ToString(),
                        Text = f.FirstName + " " + f.LastName
                    });

                    // Add a item to add new counselor to list.

                    model.CounselorList = model.CounselorList.Concat(new[] {new SelectListItem
                    {
                        Value = "0",
                        Text = "Counselor Not Listed."
                    }
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
            catch
            {
                throw;
            }
        }

        // POST: CCAs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SubmitterTypeID,StudentGradeLevel,HasExcessiveFED,ExcessiveFEDExplanation,ExcessiveFEDReasonID,CounselorID,CactusID,CounselorEmail,FirstName,LastName,Phone,ProviderID,TeacherCactusID,TeacherFirstName,TeacherLastName,OnlineCourseID,CourseCategoryID,CourseCreditID,SessionID,Comments")] CCAViewModel ccaVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CCA cca;
                    Student student;

                    // Map ViewModel to CCA
                    MapModel(ccaVm, out cca, out student);

                    //If school counselor is not found create a new one.
                    await CounselorCreate(ccaVm, cca, student).ConfigureAwait(false);

                    var session = await db.Session.FindAsync(cca.SessionID).ConfigureAwait(false);

                    // Pull out the Fiscal year from the session (i.e. Winter (2015-2016) we will put out 2016
                    cca.FiscalYear = GetFiscalYear(session);


                    db.CCAs.Add(cca);
                    var count = await db.SaveChangesAsync().ConfigureAwait(false);


                    // Send out email notifications to parent and counselor
                    if (count > 0)
                    {
                        await Notify(cca);
                    }



                    return View("Success");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error in registration process!  Error: " + ex.Message;
                    return View("Error");
                }
            }

            //Capture errors from Modelstate and return to user.

            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            foreach (var error in errors)
                ModelState.AddModelError("", error.Select(x => x.ErrorMessage).First());

            ccaVm = GetSelectLists(ccaVm);

            return View(ccaVm);

        }

        /// <summary>
        /// Notifies specified counslor and parent of new CCA being received and processed.
        /// </summary>
        /// <param name="cca"></param>
        /// <returns></returns>
        private async Task Notify(CCA cca)
        {

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (cca.CounselorID != null)
            {
                var adminRole = await db.Roles.Where(m => m.Name == "Admin").Select(m => m.Id).FirstOrDefaultAsync().ConfigureAwait(false);
                var admin = await db.Users.Where(m => m.Roles.Select(r => r.RoleId).Contains(adminRole)).FirstOrDefaultAsync().ConfigureAwait(false);

                await userManager.SendEmailAsync(admin.Id, "New SEOP Application", "A new application for SOEP has been received for " + cca.Student.StudentEmail);
                // Uncomment when testing is done and we roll out to production.

                //var counselor = await db.Counselors.FindAsync(cca.CounselorID);
                //await UserManager.SendEmailAsync(counselor.UserId, "A new application for SEOP has been received for a student at your school.");
            }
            else
            {
                var adminRole = await db.Roles.Where(m => m.Name == "Admin").Select(m => m.Id).FirstOrDefaultAsync().ConfigureAwait(false);
                var admin = await db.Users.Where(m => m.Roles.Select(r => r.RoleId).Contains(adminRole)).FirstOrDefaultAsync().ConfigureAwait(false);
                await userManager.SendEmailAsync(admin.Id, "New SEOP Application", "A new application for SOEP has been received for " + cca.Student.StudentEmail + " but no counselor has been identified.");
            }

            //Email Parent

            IdentityMessage msg = new IdentityMessage();
            var parent = await db.Parents.FindAsync(cca.Student.ParentID).ConfigureAwait(false);

            msg.Destination = parent.GuardianEmail;
            msg.Subject = "SEOP Application Received.";
            msg.Body = "An application has been received for a course in the Utah Online Public Education Program.";

            EmailService emailService = new EmailService();
            await emailService.SendAsync(msg).ConfigureAwait(false);

            //Email Provider
            var provider = await db.Providers.FindAsync(cca.ProviderID).ConfigureAwait(false);
            msg.Destination = provider.Email;
            msg.Subject = "Enrollment Request for Provider Review";
            msg.Body = "USOE has received a CCA for " + cca.Student.StudentFirstName + " " + cca.Student.StudentLastName + " who wishes to enroll in a course or courses under the SOEP.\n " + ". Please review these CCAs within 72 Business Hours.  https://seats.schools.utah.gov/";
            await emailService.SendAsync(msg).ConfigureAwait(false);
        }




        /// <summary>
        /// Gets all numeric chars out of string and tries to find Year
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private int GetFiscalYear(Models.Session session)
        {
            if (session != null && session.Name != null)
            {
                var fiscalYear = new string(session.Name.Where(c => char.IsDigit(c)).ToArray());

                fiscalYear = fiscalYear.Substring(fiscalYear.Length - 4);

                return Convert.ToInt32(fiscalYear);
            }

            throw new NullReferenceException();
        }



        /// <summary>
        /// Creates Counselor if no matching counselor was found in database.
        /// </summary>
        /// <param name="ccaVm"></param>
        /// <param name="cca"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        private async Task CounselorCreate(CCAViewModel ccaVm, CCA cca, Student student)
        {
            try
            {
                if (ccaVm.EnrollmentLocationID == 2 || (cca.EnrollmentLocationID != 1 && cca.CounselorID == 0))
                {
                    int counselorId = await db.Counselors.Where(x => x.Email == ccaVm.CounselorEmail).Select(x => x.ID).FirstOrDefaultAsync().ConfigureAwait(false);

                    if (counselorId == 0)
                    {
                        var counselor = new Counselor()
                        {
                            Email = ccaVm.CounselorEmail,
                            FirstName = ccaVm.CounselorFirstName,
                            LastName = ccaVm.CounselorLastName,
                            Phone = ccaVm.CounselorPhoneNumber
                        };

                        if (cca.EnrollmentLocationID != 2)
                        {
                            counselor.EnrollmentLocationSchoolNameID = student.EnrollmentLocationSchoolNamesID;
                        }
                        else
                        {
                            counselor.School = student.SchoolOfRecord;
                        }

                        db.Counselors.Add(counselor);

                        await db.SaveChangesAsync().ConfigureAwait(false);
                        cca.CounselorID = await db.Counselors.Where(m => m.Email == ccaVm.CounselorEmail).Select(m => m.ID).FirstOrDefaultAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        cca.CounselorID = counselorId;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Maps the model from the View Model for saving a new CCA to the database.
        /// </summary>
        /// <param name="ccaVm"></param>
        /// <param name="cca"></param>
        /// <param name="student"></param>
        private void MapModel(CCAViewModel ccaVm, out CCA cca, out Student student)
        {
            try
            {
                Mapper.CreateMap<CCAViewModel, CCA>();

                cca = Mapper.Map<CCAViewModel, CCA>(ccaVm);
                cca.ApplicationSubmissionDate = DateTime.Now;
                var userId = User.Identity.GetUserId();
                cca.UserId = userId;
                student = db.Students.Where(x => x.UserId == userId).First();
                cca.StudentID = student.ID;
                cca.EnrollmentLocationID = student.EnrollmentLocationID;
                cca.EnrollmentLocationSchoolNamesID = student.EnrollmentLocationSchoolNamesID;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets Categories from database using the sessionID.
        /// Check courses that are associated with the session and only return categories
        /// that have courses in that session.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>select list in json format</returns>
        public async Task<JsonResult> GetCategories(int sessionId)
        {
            try
            {
                IEnumerable<int> categorySelected;

                var courses = await db.Courses.ToListAsync().ConfigureAwait(false);

                var session = await db.Session.FindAsync(sessionId).ConfigureAwait(false);
                var selected = session.Name;

                // If session is a summer session only show categories that have classes for summer
                if (selected.ToLower().Contains("sum"))
                {
                    categorySelected = courses.Where(x => x.SessionID == sessionId).Select(x => x.CourseCategoryID).Distinct();
                }
                else // Show all non summer categories
                {
                    categorySelected = courses.Where(x => x.SessionID == sessionId || x.SessionID == 0).Select(x => x.CourseCategoryID).Distinct();
                }

                var categoryList = db.CourseCategories.Where(y => categorySelected.Contains(y.ID) && y.IsActive).Select(f => new SelectListItem
                    {
                        Value = f.ID.ToString(),
                        Text = f.Name
                    });

                return Json(new SelectList(categoryList, "Value", "Text"));
            }
            catch
            {
                throw new HttpException(500, "Error processing category list request.");
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
                var session = await db.Session.FindAsync(sessionId).ConfigureAwait(false);

                var courses = await db.Courses.Where(x => (x.CourseCategory.ID == categoryId && x.IsActive && x.Provider.IsActive && x.Session.IsActive)).ToListAsync();

                var selected = session.Name;

                IEnumerable<SelectListItem> courseNameList;
                if (selected.ToLower().Contains("sum")) // Get summer courses
                {
                    courseNameList = courses.Where(x => x.SessionID == sessionId).Select(f => new SelectListItem
                    {
                        Value = f.ID.ToString(),
                        Text = f.Name + " - " + f.Provider.Name
                    });
                }
                else // Get non summer courses
                {
                    courseNameList = courses.Where(x => (x.SessionID == 0 || x.SessionID == sessionId)).Select(f => new SelectListItem
                    {
                        Value = f.ID.ToString(),
                        Text = f.Name + " - " + f.Provider.Name
                    });
                }

                return Json(new SelectList(courseNameList, "Value", "Text"));
            }
            catch
            {
                throw new HttpException(500, "Error processing course list request.");
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
                var course = await db.Courses.FindAsync(courseId).ConfigureAwait(false);

                var courseResult = new CourseResultModel()
                {
                    Code = course.Code,
                    Name = course.Provider.Name,
                    Credit = course.Credit,
                    OnlineProviderID = course.ProviderID,
                    Notes = course.Notes
                };

                courseResult.CreditChoices = await GetCourseCredit(courseResult.Credit).ConfigureAwait(false);

                return (Json(courseResult));
            }
            catch
            {
                throw new HttpException(500, "Error processing course information request.");

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
            try
            {
                var creditOptions = await db.CourseCredits.ToListAsync().ConfigureAwait(false);

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
            catch
            {
                throw;
            }
        }


        // GET: CCAs/Edit/5

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                CCA cCA = await db.CCAs.FindAsync(id).ConfigureAwait(false);
                if (cCA == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CounselorID = new SelectList(db.Counselors, "ID", "Email", cCA.CounselorID);
                ViewBag.CourseID = new SelectList(db.Courses, "ID", "Name", cCA.OnlineCourseID);
                ViewBag.CourseCreditID = new SelectList(db.CourseCredits, "ID", "ID", cCA.CourseCreditID);

                return View(cCA);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error retrieving CCA List to Edit!  Error: " + ex.Message;
                return View("Error");
            }
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
                try
                {
                    Mapper.CreateMap<CCAViewModel, CCA>();

                    CCA cca = Mapper.Map<CCAViewModel, CCA>(ccaVm);
                    db.Entry(cca).State = EntityState.Modified;

                    await db.SaveChangesAsync().ConfigureAwait(false);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error processing CCA save! Error: " + ex.Message;
                    return View("Error");
                }
            }

            ViewBag.CounselorID = new SelectList(db.Counselors, "ID", "Email", ccaVm.CounselorID);
            ViewBag.CourseID = new SelectList(db.Courses, "ID", "Name", ccaVm.OnlineCourseID);
            ViewBag.CourseCreditID = new SelectList(db.CourseCredits, "ID", "ID", ccaVm.CourseCreditID);

            return View(ccaVm);
        }

        // GET: CCAs/Details/5
        /// <summary>
        /// This method provides details of the CCA to the USOE Admin .
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UsoeDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                CCA cCA = await db.CCAs.FindAsync(id).ConfigureAwait(false);
                if (cCA == null)
                {
                    return HttpNotFound();
                }
                return View(cCA);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error retrieving details! Error: " + ex.Message;
                return View("Error");
            }
        }

        // GET: CCAs/Edit/5
        /// <summary>
        /// Allows access for the USOE Admin to edit the USOE sections of the CCA.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UsoeEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                CCA cca = await db.CCAs.FindAsync(id).ConfigureAwait(false);
                if (cca == null)
                {
                    return HttpNotFound();
                }

                Mapper.CreateMap<CCA, UsoeCcaViewModel>();

                var ccaVm = Mapper.Map<CCA, UsoeCcaViewModel>(cca);

                ccaVm.CcaID = cca.ID;

                await SetUpEditViewModel(ccaVm).ConfigureAwait(false);

                return View(ccaVm);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error retrieving CCA List to Edit!  Error: " + ex.Message;
                return View("Error");
            }
        }

        private async Task SetUpEditViewModel(UsoeCcaViewModel ccaVm)
        {

            try
            {
                var leaId = ccaVm.Student.EnrollmentLocationID;
                var schoolId = ccaVm.Student.EnrollmentLocationSchoolNamesID;

                ViewBag.Lea = await cactus.CactusInstitutions.Where(c => c.ID == leaId).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);
                ViewBag.School = await cactus.CactusSchools.Where(c => c.ID == schoolId).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);

            }
            catch
            {
                throw;
            }
        }

        private void SetUpProviderEditViewModel(ProviderCcaViewModel ccaVm)
        {

            try
            {
                var sessionList = new SelectList(db.Session, "ID", "Name", ccaVm.SessionID);
                var categoryList = new SelectList(db.CourseCategories, "ID", "Name", ccaVm.CourseCategoryID);
                var providerCourses = db.Courses.Where(m => m.ProviderID == ccaVm.ProviderID);
                var courseList = new SelectList(providerCourses, "ID", "Name", ccaVm.OnlineCourseID);

                ccaVm.CourseCreditList = new List<SelectListItem>();

                ViewBag.SessionID = sessionList;
                ViewBag.CourseCategoryID = categoryList;
                ViewBag.OnlineCourseID = courseList;

            }
            catch
            {
                throw;
            }
        }

        // POST: CCAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UsoeEdit(UsoeCcaViewModel ccaVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CCA cca = await db.CCAs.FindAsync(ccaVm.CcaID).ConfigureAwait(false);

                    Mapper.CreateMap<UsoeCcaViewModel, CCA>().ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

                    cca = Mapper.Map<UsoeCcaViewModel, CCA>(ccaVm, cca);

                    //db.Entry(cca).State = EntityState.Modified;

                    await db.SaveChangesAsync().ConfigureAwait(false);
                    return RedirectToAction("CcaInterface", "Admin");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error processing CCA save! Error: " + ex.Message;
                    return View("Error");
                }
            }

            //Capture errors from Modelstate and return to user.

            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            foreach (var error in errors)
                ModelState.AddModelError("", error.Select(x => x.ErrorMessage).First());

            await SetUpEditViewModel(ccaVm).ConfigureAwait(false);

            return View(ccaVm);
        }

        // GET: CCAs/Edit/5
        /// <summary>
        /// Allows access for the Primary User to edit the Primary sections of the CCA.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Primary")]
        public async Task<ActionResult> PrimaryEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                CCA cca = await db.CCAs.FindAsync(id).ConfigureAwait(false);
                if (cca == null)
                {
                    return HttpNotFound();
                }

                Mapper.CreateMap<CCA, PrimaryCcaViewModel>();

                var ccaVm = Mapper.Map<CCA, PrimaryCcaViewModel>(cca);

                ccaVm.CcaID = cca.ID;

                //await SetUpEditViewModel(ccaVm);

                return View(ccaVm);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error retrieving CCA List to Edit!  Error: " + ex.Message;
                return View("Error");
            }
        }

        // POST: CCAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Primary")]
        public async Task<ActionResult> PrimaryEdit(PrimaryCcaViewModel ccaVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CCA cca = await db.CCAs.FindAsync(ccaVm.CcaID).ConfigureAwait(false);
                    cca.IsBusinessAdministratorAcceptRejectEnrollment = ccaVm.IsBusinessAdministratorAcceptRejectEnrollment;
                    cca.PrimaryLEAExplantionRejection = ccaVm.PrimaryLEAExplantionRejection;
                    cca.PrimaryLEAReasonRejectingCCA = ccaVm.PrimaryLEAReasonRejectingCCA;


                    db.Entry(cca).State = EntityState.Modified;

                    await db.SaveChangesAsync().ConfigureAwait(false);
                    return RedirectToAction("CcaInterface", "PrimaryUsers");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error processing CCA save! Error: " + ex.Message;
                    return View("Error");
                }
            }

            //Capture errors from Modelstate and return to user.

            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            foreach (var error in errors)
                ModelState.AddModelError("", error.Select(x => x.ErrorMessage).First());

            //await SetUpEditViewModel(ccaVm);

            return View(ccaVm);
        }

        // GET: CCAs/Details/5
        /// <summary>
        /// This method provides details of the CCA to the Primary and Counselors.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Primary,Counselor")]
        public async Task<ActionResult> PrimaryDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                CCA cCA = await db.CCAs.FindAsync(id).ConfigureAwait(false);
                if (cCA == null)
                {
                    return HttpNotFound();
                }
                return View(cCA);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error retrieving details! Error: " + ex.Message;
                return View("Error");
            }
        }



        // GET: CCAs/Edit/5
        /// <summary>
        /// Allows access for the Provider User to the CCA with ID of id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Provider")]
        public async Task<ActionResult> ProviderEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                CCA cca = await db.CCAs.FindAsync(id).ConfigureAwait(false);
                if (cca == null)
                {
                    return HttpNotFound();
                }

                Mapper.CreateMap<CCA, ProviderCcaViewModel>();

                var ccaVm = Mapper.Map<CCA, ProviderCcaViewModel>(cca);

                ccaVm.CcaID = cca.ID;

                //ViewBag.SessionID = new SelectList(db.Session, "ID", "Name");
                //ViewBag.CourseCategoryID = new SelectList(db.CourseCategories, "ID", "Name");
                //ViewBag.OnlineCourseID = new SelectList(db.Courses, "ID", "Name");

                SetUpProviderEditViewModel(ccaVm);

                return View(ccaVm);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error retrieving CCA List to Edit!  Error: " + ex.Message;
                return View("Error");
            }
        }

        // POST: CCAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Provider")]
        public async Task<ActionResult> ProviderEdit(ProviderCcaViewModel ccaVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CCA cca = await db.CCAs.FindAsync(ccaVm.CcaID).ConfigureAwait(false);

                    // Map viewmodel to cca

                    Mapper.CreateMap<ProviderCcaViewModel, CCA>().ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

                    Mapper.Map<ProviderCcaViewModel, CCA>(ccaVm, cca);

                    db.Entry(cca).State = EntityState.Modified;

                    await db.SaveChangesAsync().ConfigureAwait(false);
                    return RedirectToAction("CcaInterface", "ProviderUsers");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error processing CCA save! Error: " + ex.Message;
                    return View("Error");
                }
            }

            //Capture errors from Modelstate and return to user.

            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            foreach (var error in errors)
                ModelState.AddModelError("", error.Select(x => x.ErrorMessage).First());

            SetUpProviderEditViewModel(ccaVm);

            return View(ccaVm);
        }

        // GET: CCAs/Details/5
        /// <summary>
        /// This method provides details of the CCA to the Provider .
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Provider")]
        public async Task<ActionResult> ProviderDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                CCA cCA = await db.CCAs.FindAsync(id).ConfigureAwait(false);
                if (cCA == null)
                {
                    return HttpNotFound();
                }
                return View(cCA);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error retrieving details! Error: " + ex.Message;
                return View("Error");
            }
        }
        // GET: CCAs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CCA cCA = await db.CCAs.FindAsync(id).ConfigureAwait(false);
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
            CCA cCA = await db.CCAs.FindAsync(id).ConfigureAwait(false);
            db.CCAs.Remove(cCA);
            await db.SaveChangesAsync().ConfigureAwait(false);
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
