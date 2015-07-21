using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;

using SEATS.Models;
using AutoMapper;

using Microsoft.AspNet.Identity.Owin;
using System.Collections;


namespace SEATS.Controllers
{
    [Authorize]
    public class CCAsController : Controller
    {

        private const short YEARDIGITS = 4; // Number of digits in the fiscal year from Session.Name

        //private SeatsContext db;
        private SEATSEntities cactus;
        private ApplicationDbContext db;

        public CCAsController()
        {
            this.db = new ApplicationDbContext();
            this.cactus = new SEATSEntities();

        }


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
            List<CourseStatusViewModel> courses = new List<CourseStatusViewModel>();
            var userId = User.Identity.GetUserId();
            try
            {
                courses = await db.CCAs.Where(m => m.UserId == userId).Select(f => new CourseStatusViewModel
                {
                    ApplicationSubmissionDate = f.ApplicationSubmissionDate,
                    CompletionStatus = f.CourseCompletionStatus.Status,
                    CourseCategoryID = f.CourseCategoryID,
                    CourseCategory = f.CourseCategory,
                    OnlineCourseID = f.OnlineCourseID,
                    OnlineCourse = f.OnlineCourse,
                    ProviderID = f.ProviderID,
                    Provider = f.Provider,
                    SessionID = f.SessionID,
                    Session = f.Session
                }).ToListAsync().ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error getting list of applications. " + ex.Message);
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
        /// <summary>
        /// Set up lists and viewmodel for creation of student CCA
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Create()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var model = new CCAViewModel();
                var leaId = await db.Students.Where(m => m.UserId == userId).Select(m => m.EnrollmentLocationID).FirstOrDefaultAsync().ConfigureAwait(false);
                model.EnrollmentLocationID = (int)leaId;
                model.UserId = userId;
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
                var leaId = model.EnrollmentLocationID;
                if (leaId == 0 || leaId == 1)
                {
                    model.CounselorList = new List<SelectListItem>();
                }
                else
                {
                    var schoolID = db.Students.Where(m => m.UserId == model.UserId).Select(m => m.EnrollmentLocationSchoolNamesID).FirstOrDefault();

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
        public async Task<ActionResult> Create([Bind(Include = "SubmitterTypeID,StudentGradeLevel,HasExcessiveFED,ExcessiveFEDExplanation,ExcessiveFEDReasonID,CounselorID,CactusID,CounselorEmail,CounselorFirstName,CounselorLastName,CounselorPhoneNumber,Phone,ProviderID,OnlineCourseID,CourseCategoryID,CourseCreditID,SessionID,Comments")] CCAViewModel ccaVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.Identity.GetUserId();
                    ccaVm.UserId = userId;

                    //Get student associated with this user
                    Student student = await db.Students.Where(x => x.UserId == userId).FirstOrDefaultAsync();

                    // Map ViewModel to CCA and student
                    CCA cca = MapModel(ccaVm, student);

                    //Assign Counselor
                    await CounselorCreate(ccaVm, cca, student).ConfigureAwait(false);

                    // Pull out the Fiscal year from the session (i.e. Winter (2015-2016) we will put out 2016
                    var session = await db.Session.FindAsync(cca.SessionID).ConfigureAwait(false);
                    cca.FiscalYear = GetFiscalYear(session.Name);

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
            try
            {
                //Email Parent

                await EmailParent(cca);

                //Email Admin
                await EmailAdmin(cca);

                //Email Counselor

                await EmailCounselor(cca);

                //Email Primary
                if (!(cca.EnrollmentLocationID == GlobalVariables.HOMESCHOOLID || cca.EnrollmentLocationID == GlobalVariables.PRIVATESCHOOLID))
                    await EmailPrimary(cca);

                //Email Provider 

                await EmailProvider(cca);
            }
            catch
            {
                throw;
            }
        }

        private async Task EmailProvider(CCA cca)
        {
            try
            {
                IdentityMessage msg = new IdentityMessage();
                var provider = await db.Providers.FindAsync(cca.ProviderID).ConfigureAwait(false);
                msg.Destination = provider.Email;
                msg.Subject = "Enrollment Request for Provider Review";
                var initial = cca.Student.StudentFirstName[0];
                msg.Body = "<p>USOE has received a CCA for " + initial + ". " + cca.Student.StudentLastName + ", who wishes to enroll in a course under the Statewide Online Education Program.</p> <p> Please review this CCA within 72 Business Hours.  https://seats.schools.utah.gov/ </p><p>1. Login.</p><p>1. Click the edit button for that student.</p>";

                EmailService emailService = new EmailService();
                await emailService.SendAsync(msg).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }

        }

        private async Task EmailParent(CCA cca)
        {
            try
            {
                IdentityMessage msg = new IdentityMessage();

                var parent = await db.Parents.FindAsync(cca.Student.ParentID).ConfigureAwait(false);

                msg.Destination = parent.GuardianEmail;
                msg.Subject = "SEOP Application Received.";
                msg.Body = "<p>Dear Parent or Guardian,</p> <p>An \"Enrollment Request\" for online enrollment in a course taught by a lea district or charter lea outside of your primary lea district or charter lea has been filed with the Utah State Office of Education (USOE) for your child.  WITHIN APPROXIMATELY 72 BUSINESS HOURS (approximately 10 business days), YOU CAN EXPECT TO RECEIVE A NOTICE OF ENROLLMENT or REJECTION THIS STUDENT. If you have any questions in this period, please feel free to contact USOE as below.</p> <p>Please call us at 801.538.7660 with any questions.</p>On behalf of the Utah State Office of Education, we thank you for your assistance. </p><p>E-Mail:				edonline@schools.utah.gov </p><p>Tel.: 801.538.7660.</p><p>More information can be found at https://seats.schools.utah.gov/ </p>";

                EmailService emailService = new EmailService();
                await emailService.SendAsync(msg).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }

        }

        private async Task EmailAdmin(CCA cca)
        {
            try
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var adminRole = await db.Roles.Where(m => m.Name == "Admin").Select(m => m.Id).FirstOrDefaultAsync().ConfigureAwait(false);
                var admin = await db.Users.Where(m => m.Roles.Select(r => r.RoleId).Contains(adminRole)).FirstOrDefaultAsync().ConfigureAwait(false);

                var initial = cca.Student.StudentFirstName[0];
                await userManager.SendEmailAsync(admin.Id, "Admin Notification:New SEOP Application", "EDONLINE, A new application for SOEP has been received from <p>" + initial + ". " + cca.Student.StudentLastName + " Email:" + cca.Student.StudentEmail + "</p><p>This email was also sent to:</p><p>Parent:" + cca.Student.Parent.GuardianEmail + "</p>");
            }
            catch
            {
                throw;
            }
        }

        private async Task EmailCounselor(CCA cca)
        {
            try
            {
                IdentityMessage msg = new IdentityMessage();

                var counselor = await db.Counselors.FindAsync(cca.CounselorID);
                var initial = cca.Student.StudentFirstName[0];
                msg.Destination = counselor.Email;
                msg.Subject = "SEOP Application Received.";
                msg.Body = "<p>Dear Counselor,</p> <p>One or more enrollment requests have been submitted on behalf of a student enrolled in your district for this lea year. The student is:<p>First Name: " + initial + ".</p><p> Last Name: " + cca.Student.StudentLastName + "</p><p>State Board of Education Administrative Rule requires that a counselor designated by the primary lea of enrollment shall review the CCA to ensure consistency with graduation requirements, IEP and IB program participation, if applicable.</p><p>To facilitate Counselor review, the Counselor is expected to log in to certify that the course is consistent with the student’s College and Career Ready Planning (SEOP/CCRP), IEP, and IB participation, which will aid your LEA’s approving official (usually the Business Administrator) in their acceptance of this Acknowledgement.</p><p>Please go to https://seats.schools.utah.gov/ and log in to see the application.  If you do not have an account please register as a \"Counselor\" and wait for approval from the SEOP administrator.</p><p>For questions you are welcome to contact us at 801.538.7660 or edonline@schools.utah.gov.</p><p> Thank you for your assistance.</p><p>With Best Wishes,</p><p>EdOnline. http://www.schools.utah.gov/edonline/default.aspx </p>";

                EmailService emailService = new EmailService();
                await emailService.SendAsync(msg).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }

        }

        private async Task EmailPrimary(CCA cca)
        {
            try
            {
                var primaryUsers = await db.PrimaryUsers.Where(m => m.EnrollmentLocationID == cca.EnrollmentLocationID).ToListAsync();
                if (primaryUsers.Count != 0)
                {
                    foreach (var user in primaryUsers)
                    {
                        var initial = cca.Student.StudentFirstName[0];
                        IdentityMessage msg = new IdentityMessage();
                        msg.Destination = user.Email;
                        msg.Subject = "SEOP Application Received.";
                        msg.Body = "<p>Dear Primary Representative or Business Administrator,</p> <p>One or more enrollment requests have been submitted on behalf of a student enrolled in your district for this lea year. The student is:<p>" + initial + ". " + cca.Student.StudentLastName + "</p><p> The student is limited to 5.0 online credits in the 2015-16 lea year, and 6.0 credits thereafter, unless you wish to allow the student to exceed this value.</p><p>State Board of Education Administrative Rule requires that a counselor designated by the primary lea of enrollment shall review the CCA to ensure consistency with graduation requirements, IEP and IB program participation, if applicable. Statute allows 72 business hours from sending of this notice by USOE that an enrollment request is pending for your review, prior to execution of an enrollment request by USOE. At that point, the student is considered to be enrolled if they have been accepted by the Provider, and you have either approved the enrollment, or failed to disapprove the enrollment on statutory bases.</p><p>To facilitate Counselor review, the student’s assigned Counselor has been similarly notified that an enrollment request is pending.  The Counselor is expected to log in to certify that the course is consistent with the student’s College and Career Ready Planning (SEOP/CCRP), IEP, and IB participation, which will aid in your own evaluation of the request.</p><p>Please go to https://seats.schools.utah.gov/ and log in to see the application.  If you do not have an account please register as a \"Primary\" and wait for approval from the SEOP administrator.</p><p>For questions you are welcome to contact us at 801.538.7660 or edonline@schools.utah.gov.</p><p> Thank you for your assistance.</p><p>With Best Wishes,</p><p>EdOnline.</p>";

                        EmailService emailService = new EmailService();
                        await emailService.SendAsync(msg).ConfigureAwait(false);
                    }
                }
                else // Email admin if no primary user found
                {
                    var initial = cca.Student.StudentFirstName[0];
                    var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var adminRole = await db.Roles.Where(m => m.Name == "Admin").Select(m => m.Id).FirstOrDefaultAsync().ConfigureAwait(false);
                    var admin = await db.Users.Where(m => m.Roles.Select(r => r.RoleId).Contains(adminRole)).FirstOrDefaultAsync().ConfigureAwait(false);
                    var school = await cactus.CactusInstitutions.Where(m => m.ID == cca.EnrollmentLocationID).FirstAsync();
                    await userManager.SendEmailAsync(admin.Id, "NO PRIMARY USER FOUND!", "EDONLINE, A new application for SOEP has been received from <p>" + initial + ". " + cca.Student.StudentLastName + " Email:" + cca.Student.StudentEmail + "</p><p>But a Primary user was not found for " + school + "</p>");
                }
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Gets all numeric chars out of string and tries to find Year
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private int GetFiscalYear(string session)
        {
            if (session != null)
            {
                var fiscalYear = new string(session.Where(c => char.IsDigit(c)).ToArray());
                if (fiscalYear.Length >= YEARDIGITS)
                    fiscalYear = fiscalYear.Substring(fiscalYear.Length - YEARDIGITS);

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
            // Homeschoolers will be assigned Counselor at the ID=0 entry (now Cory Kanth)
            try
            {
                if (cca.EnrollmentLocationID == GlobalVariables.PRIVATESCHOOLID || (cca.EnrollmentLocationID != GlobalVariables.HOMESCHOOLID && cca.CounselorID == 0))
                {
                    int counselorId = 0;
                    // Check for existing counselor entry
                    if (ccaVm.CounselorEmail != null)
                    {
                        counselorId = await db.Counselors.Where(x => x.Email == ccaVm.CounselorEmail).Select(x => x.ID).FirstOrDefaultAsync().ConfigureAwait(false);
                    }

                    if (counselorId == 0)
                    {
                        var counselor = new Counselor()
                        {
                            Email = ccaVm.CounselorEmail,
                            FirstName = ccaVm.CounselorFirstName,
                            LastName = ccaVm.CounselorLastName,
                            Phone = ccaVm.CounselorPhoneNumber
                        };

                        if (cca.EnrollmentLocationID != GlobalVariables.PRIVATESCHOOLID)
                        {
                            counselor.EnrollmentLocationID = cca.EnrollmentLocationID;
                            counselor.EnrollmentLocationSchoolNameID = student.EnrollmentLocationSchoolNamesID;
                            var school = await cactus.CactusSchools.Where(m => m.ID == student.EnrollmentLocationSchoolNamesID).FirstOrDefaultAsync().ConfigureAwait(false);

                            counselor.School = school.Name;

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
        private CCA MapModel(CCAViewModel ccaVm, Student student)
        {
            try
            {
                Mapper.CreateMap<CCAViewModel, CCA>();
                var cca = Mapper.Map<CCAViewModel, CCA>(ccaVm);
                cca.ApplicationSubmissionDate = DateTime.Now;
                cca.StudentID = student.ID;
                cca.EnrollmentLocationID = student.EnrollmentLocationID;
                cca.EnrollmentLocationSchoolNamesID = student.EnrollmentLocationSchoolNamesID;
                return cca;
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
                else // Show all non summer categories 0 is All Sessions
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

                await SetUpUsoeEditViewModel(ccaVm).ConfigureAwait(false);

                return View(ccaVm);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error retrieving CCA List to Edit!  Error: " + ex.Message;
                return View("Error");
            }
        }

        private async Task SetUpUsoeEditViewModel(UsoeCcaViewModel ccaVm)
        {

            try
            {
                var status = new SelectList(await db.CourseCompletionStatus.ToListAsync().ConfigureAwait(false), "ID", "Status", ccaVm.CourseCompletionStatusID);
                ViewBag.CourseCompletionStatusID = status;

                ccaVm.CourseCreditList = await GetCourseCredit(ccaVm.OnlineCourse.Credit);

                var leaId = ccaVm.Student.EnrollmentLocationID;
                var schoolId = ccaVm.Student.EnrollmentLocationSchoolNamesID;
                if (leaId == 1) // HomeSchool
                {
                    ViewBag.Lea = "HOMESCHOOL";
                    ViewBag.School = "HOMESCHOOL";
                }
                else if (leaId == 2) //PrivateSchool
                {
                    ViewBag.Lea = "PRIVATESCHOOL";
                    ViewBag.School = ccaVm.Student.SchoolOfRecord;
                }
                else
                {
                    ViewBag.Lea = await cactus.CactusInstitutions.Where(c => c.ID == leaId).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);

                    if (schoolId != null)
                        ViewBag.School = await cactus.CactusSchools.Where(c => c.ID == schoolId).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);
                    else
                        ViewBag.School = "UNKNOWN";
                }

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

                    // Pull out the Fiscal year from the session (i.e. Winter (2015-2016) we will put out 2016

                    var session = await db.Session.FindAsync(cca.SessionID).ConfigureAwait(false);
                    cca.FiscalYear = GetFiscalYear(session.Name);

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

            await SetUpUsoeEditViewModel(ccaVm).ConfigureAwait(false);

            return View(ccaVm);
        }

        // GET: CCAs/Edit/5
        /// <summary>
        /// Allows access for the Primary User to edit the Primary sections of the CCA.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Counselor")]
        public async Task<ActionResult> CounselorEdit(int? id)
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

                Mapper.CreateMap<CCA, CounselorCcaViewModel>();

                var ccaVm = Mapper.Map<CCA, CounselorCcaViewModel>(cca);

                ccaVm.CcaID = cca.ID;

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
        [Authorize(Roles = "Counselor")]
        public async Task<ActionResult> CounselorEdit(CounselorCcaViewModel ccaVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CCA cca = await db.CCAs.FindAsync(ccaVm.CcaID).ConfigureAwait(false);
                    cca.IsCounselorSigned = ccaVm.IsCounselorSigned;
                    cca.IsCourseConsistentWithStudentSEOP = ccaVm.IsCourseConsistentWithStudentSEOP;

                    db.Entry(cca).State = EntityState.Modified;

                    await db.SaveChangesAsync().ConfigureAwait(false);
                    return RedirectToAction("CcaInterface", "Counselors");
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

                var reasons = await db.PrimaryRejectionReasons.ToListAsync().ConfigureAwait(false);
                ViewBag.PrimaryRejectionReasonsID = new SelectList(reasons, "ID", "Reason", ccaVm.PrimaryRejectionReasonsID);

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
                    cca.PrimaryRejectionReasonsID = ccaVm.PrimaryRejectionReasonsID;
                    cca.PrimaryNotes = ccaVm.PrimaryNotes;
                    cca.BusinessAdministratorSignature = ccaVm.BusinessAdministratorSignature;

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

                if (cCA.OnlineCourseID != 0)
                {
                    var course = await db.Courses.FindAsync(cCA.OnlineCourseID).ConfigureAwait(false);
                    ViewBag.CoreCode = course.Code;
                }
                else
                    ViewBag.CoreCode = " ";

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

                var ccaVm = await SetUpProviderEditViewModel(id);

                return View(ccaVm);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error retrieving CCA List to Edit!  Error: " + ex.Message;
                return View("Error");
            }
        }

        private async Task<ProviderCcaViewModel> SetUpProviderEditViewModel(int? id)
        {

            try
            {
                CCA cca = await db.CCAs.FindAsync(id).ConfigureAwait(false);

                Mapper.CreateMap<CCA, ProviderCcaViewModel>();

                var ccaVm = Mapper.Map<CCA, ProviderCcaViewModel>(cca);

                ccaVm.CcaID = cca.ID;
                var sessionList = new SelectList(db.Session, "ID", "Name", ccaVm.SessionID);
                var categoryList = new SelectList(db.CourseCategories, "ID", "Name", ccaVm.CourseCategoryID);
                var providerCourses = db.Courses.Where(m => m.ProviderID == ccaVm.ProviderID);
                var courseList = new SelectList(providerCourses, "ID", "Name", ccaVm.OnlineCourseID);
                var credit = await db.CourseCredits.Where(m => m.ID == ccaVm.CourseCreditID).ToListAsync();
                ccaVm.CourseCreditList = new SelectList(credit, "ID", "Value", ccaVm.CourseCreditID);

                ViewBag.SessionID = sessionList;
                ViewBag.CourseCategoryID = categoryList;
                ViewBag.OnlineCourseID = courseList;

                var reasons = await db.ProviderRejectionReasons.ToListAsync().ConfigureAwait(false);
                ViewBag.ProviderRejectionReasonsID = new SelectList(reasons, "ID", "Reason");
                var status = new SelectList(await db.CourseCompletionStatus.ToListAsync().ConfigureAwait(false), "ID", "Status", ccaVm.CourseCompletionStatusID);
                ViewBag.CourseCompletionStatusID = status;

                return ccaVm;

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

                    // In case the session changed we need to change FiscalYear
                    // Pull out the Fiscal year from the session (i.e. Winter (2015-2016) we will put out 2016

                    var session = await db.Session.FindAsync(cca.SessionID).ConfigureAwait(false);
                    cca.FiscalYear = GetFiscalYear(session.Name);

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



            return View(await SetUpProviderEditViewModel(ccaVm.CcaID));
        }
     
        /// <summary>
        /// This method is called from the ProviderUser to bulk update CCAs.  All the selected items will be updated to the same value.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> SaveBulkUpdate()
        {
            try
            {
                // rowIds are the select rows
                var rowIds = TempData["RowIds"] as int[];

                // rows to Edit contain the updated values for the bulk update
                var rowsToEdit = TempData["RowsToEdit"] as ProviderCcaVmList;
             
                var updatedRows = await db.CCAs.Where(m => rowIds.Contains(m.ID)).ToListAsync();

                if (rowsToEdit.BulkEdit.CourseCompletionStatusID != 0)
                    rowsToEdit.BulkEdit.CourseCompletionStatus = await db.CourseCompletionStatus.FindAsync(rowsToEdit.BulkEdit.CourseCompletionStatusID).ConfigureAwait(false);

                foreach (var row in updatedRows)
                {
                    if (rowsToEdit.BulkEdit.CourseCompletionStatus != null) row.CourseCompletionStatus = rowsToEdit.BulkEdit.CourseCompletionStatus;
                    if (rowsToEdit.BulkEdit.CourseCompletionDate != null) row.CourseCompletionDate = rowsToEdit.BulkEdit.CourseCompletionDate;
                    if (rowsToEdit.BulkEdit.CourseStartDate != null) row.CourseStartDate = rowsToEdit.BulkEdit.CourseStartDate;
                    if (rowsToEdit.BulkEdit.DateConfirmationActiveParticipation != null) row.DateConfirmationActiveParticipation = rowsToEdit.BulkEdit.DateConfirmationActiveParticipation;

                }

                await db.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction("CcaInterface", "ProviderUsers");
                
            }
            catch
            {
                throw new HttpException(500, "Error processing course information request.");

            }
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

                if (cCA.OnlineCourseID != 0)
                {
                    var course = await db.Courses.FindAsync(cCA.OnlineCourseID).ConfigureAwait(false);
                    ViewBag.CoreCode = course.Code;
                }
                else
                    ViewBag.CoreCode = " ";

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
