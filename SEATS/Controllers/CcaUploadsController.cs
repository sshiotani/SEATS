using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEATS.Models;
using OfficeOpenXml;
using Microsoft.AspNet.Identity.Owin;

namespace SEATS.Controllers
{
    public class CcaUploadsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private SEATSEntities cactus = new SEATSEntities();

        // GET: CcaUploads
        public async Task<ActionResult> Index()
        {
            var cCAs = db.CCAs.Include(c => c.Counselor).Include(c => c.CounselorRejectionReasons).Include(c => c.CourseCategory).Include(c => c.CourseCompletionStatus).Include(c => c.CourseCredit).Include(c => c.OnlineCourse).Include(c => c.PrimaryRejectionReasons).Include(c => c.Provider).Include(c => c.ProviderRejectionReasons).Include(c => c.Session).Include(c => c.Student);
            return View(await cCAs.ToListAsync());
        }

        // GET: CcaUploads/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CcaUpload ccaUpload = await db.CcaUpload.FindAsync(id);
            if (ccaUpload == null)
            {
                return HttpNotFound();
            }
            return View(ccaUpload);
        }

        // GET: CcaUploads/Create
        public ActionResult Create()
        {
            ViewBag.CounselorID = new SelectList(db.Counselors, "ID", "UserId");
            ViewBag.CounselorRejectionReasonsID = new SelectList(db.CounselorRejectionReasons, "ID", "Reason");
            ViewBag.CourseCategoryID = new SelectList(db.CourseCategories, "ID", "Name");
            ViewBag.CourseCompletionStatusID = new SelectList(db.CourseCompletionStatus, "ID", "Status");
            ViewBag.CourseCreditID = new SelectList(db.CourseCredits, "ID", "Value");
            ViewBag.OnlineCourseID = new SelectList(db.Courses, "ID", "Name");
            ViewBag.PrimaryRejectionReasonsID = new SelectList(db.PrimaryRejectionReasons, "ID", "Reason");
            ViewBag.ProviderID = new SelectList(db.Providers, "ID", "Name");
            ViewBag.ProviderRejectionReasonsID = new SelectList(db.ProviderRejectionReasons, "ID", "Reason");
            ViewBag.SessionID = new SelectList(db.Session, "ID", "Name");
            ViewBag.StudentID = new SelectList(db.Students, "ID", "UserId");
            return View();
        }

        // POST: CcaUploads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,SubmitterTypeID,ApplicationSubmissionDate,FiscalYear,UserId,StudentID,StudentGradeLevel,HasExcessiveFED,ExcessiveFEDExplanation,ExcessiveFEDReasonID,Comments,EnrollmentLocationSchoolNamesID,CounselorID,IsCounselorSigned,IsCounselorRejecting,CounselorRejectionReasonsID,CounselorRejectionExplantion,EnrollmentLocationID,IsBusinessAdministratorAcceptRejectEnrollment,PrimaryRejectionReasonsID,PrimaryLEAExplantionRejection,DateBusinessAdministratorSignature,BusinessAdministratorSignature,PrimaryNotes,ProviderID,TeacherCactusID,TeacherFirstName,TeacherLastName,CourseCompletionStatusID,CourseBegin,CourseStartDate,CourseCompletionDate,CreditCompletedToDate,DateConfirmationActiveParticipation,DateContinuationActiveParticipation,DateReportPassingGrade,IsEnrollmentNoticeSent,IsProviderAcceptsRejectsCourseRequest,IsProviderEnrollmentVerified,ProviderSignature,IsProviderSignature,ProviderExplanationRejection,ProviderRejectionReasonsID,ProviderNotes,OnlineCourseID,CourseCategoryID,CourseFee,CourseCreditID,CourseName2ndSemesterID,IsCourseConsistentWithStudentSEOP,SessionID,BudgetPrimaryProvider,IsRemediation,NotificationDate,PrimaryNotificationDate,PriorDisbursementProvider,RecordNotes,RemediationPeriodBegins,TotalDisbursementsProvider,TwentyDaysPastSemesterStartDate,Offset,Distribution,WithdrawalDate,Grand_Total,Notes")] CcaUpload ccaUpload)
        {
            if (ModelState.IsValid)
            {
                db.CCAs.Add(ccaUpload);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CounselorID = new SelectList(db.Counselors, "ID", "UserId", ccaUpload.CounselorID);
            ViewBag.CounselorRejectionReasonsID = new SelectList(db.CounselorRejectionReasons, "ID", "Reason", ccaUpload.CounselorRejectionReasonsID);
            ViewBag.CourseCategoryID = new SelectList(db.CourseCategories, "ID", "Name", ccaUpload.CourseCategoryID);
            ViewBag.CourseCompletionStatusID = new SelectList(db.CourseCompletionStatus, "ID", "Status", ccaUpload.CourseCompletionStatusID);
            ViewBag.CourseCreditID = new SelectList(db.CourseCredits, "ID", "Value", ccaUpload.CourseCreditID);
            ViewBag.OnlineCourseID = new SelectList(db.Courses, "ID", "Name", ccaUpload.OnlineCourseID);
            ViewBag.PrimaryRejectionReasonsID = new SelectList(db.PrimaryRejectionReasons, "ID", "Reason", ccaUpload.PrimaryRejectionReasonsID);
            ViewBag.ProviderID = new SelectList(db.Providers, "ID", "Name", ccaUpload.ProviderID);
            ViewBag.ProviderRejectionReasonsID = new SelectList(db.ProviderRejectionReasons, "ID", "Reason", ccaUpload.ProviderRejectionReasonsID);
            ViewBag.SessionID = new SelectList(db.Session, "ID", "Name", ccaUpload.SessionID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "UserId", ccaUpload.StudentID);
            return View(ccaUpload);
        }

        // GET: CcaUploads/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CcaUpload ccaUpload = await db.CcaUpload.FindAsync(id);
            if (ccaUpload == null)
            {
                return HttpNotFound();
            }
            ViewBag.CounselorID = new SelectList(db.Counselors, "ID", "UserId", ccaUpload.CounselorID);
            ViewBag.CounselorRejectionReasonsID = new SelectList(db.CounselorRejectionReasons, "ID", "Reason", ccaUpload.CounselorRejectionReasonsID);
            ViewBag.CourseCategoryID = new SelectList(db.CourseCategories, "ID", "Name", ccaUpload.CourseCategoryID);
            ViewBag.CourseCompletionStatusID = new SelectList(db.CourseCompletionStatus, "ID", "Status", ccaUpload.CourseCompletionStatusID);
            ViewBag.CourseCreditID = new SelectList(db.CourseCredits, "ID", "Value", ccaUpload.CourseCreditID);
            ViewBag.OnlineCourseID = new SelectList(db.Courses, "ID", "Name", ccaUpload.OnlineCourseID);
            ViewBag.PrimaryRejectionReasonsID = new SelectList(db.PrimaryRejectionReasons, "ID", "Reason", ccaUpload.PrimaryRejectionReasonsID);
            ViewBag.ProviderID = new SelectList(db.Providers, "ID", "Name", ccaUpload.ProviderID);
            ViewBag.ProviderRejectionReasonsID = new SelectList(db.ProviderRejectionReasons, "ID", "Reason", ccaUpload.ProviderRejectionReasonsID);
            ViewBag.SessionID = new SelectList(db.Session, "ID", "Name", ccaUpload.SessionID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "UserId", ccaUpload.StudentID);
            return View(ccaUpload);
        }

        // POST: CcaUploads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,SubmitterTypeID,ApplicationSubmissionDate,FiscalYear,UserId,StudentID,StudentGradeLevel,HasExcessiveFED,ExcessiveFEDExplanation,ExcessiveFEDReasonID,Comments,EnrollmentLocationSchoolNamesID,CounselorID,IsCounselorSigned,IsCounselorRejecting,CounselorRejectionReasonsID,CounselorRejectionExplantion,EnrollmentLocationID,IsBusinessAdministratorAcceptRejectEnrollment,PrimaryRejectionReasonsID,PrimaryLEAExplantionRejection,DateBusinessAdministratorSignature,BusinessAdministratorSignature,PrimaryNotes,ProviderID,TeacherCactusID,TeacherFirstName,TeacherLastName,CourseCompletionStatusID,CourseBegin,CourseStartDate,CourseCompletionDate,CreditCompletedToDate,DateConfirmationActiveParticipation,DateContinuationActiveParticipation,DateReportPassingGrade,IsEnrollmentNoticeSent,IsProviderAcceptsRejectsCourseRequest,IsProviderEnrollmentVerified,ProviderSignature,IsProviderSignature,ProviderExplanationRejection,ProviderRejectionReasonsID,ProviderNotes,OnlineCourseID,CourseCategoryID,CourseFee,CourseCreditID,CourseName2ndSemesterID,IsCourseConsistentWithStudentSEOP,SessionID,BudgetPrimaryProvider,IsRemediation,NotificationDate,PrimaryNotificationDate,PriorDisbursementProvider,RecordNotes,RemediationPeriodBegins,TotalDisbursementsProvider,TwentyDaysPastSemesterStartDate,Offset,Distribution,WithdrawalDate,Grand_Total,Notes")] CcaUpload ccaUpload)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ccaUpload).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CounselorID = new SelectList(db.Counselors, "ID", "UserId", ccaUpload.CounselorID);
            ViewBag.CounselorRejectionReasonsID = new SelectList(db.CounselorRejectionReasons, "ID", "Reason", ccaUpload.CounselorRejectionReasonsID);
            ViewBag.CourseCategoryID = new SelectList(db.CourseCategories, "ID", "Name", ccaUpload.CourseCategoryID);
            ViewBag.CourseCompletionStatusID = new SelectList(db.CourseCompletionStatus, "ID", "Status", ccaUpload.CourseCompletionStatusID);
            ViewBag.CourseCreditID = new SelectList(db.CourseCredits, "ID", "Value", ccaUpload.CourseCreditID);
            ViewBag.OnlineCourseID = new SelectList(db.Courses, "ID", "Name", ccaUpload.OnlineCourseID);
            ViewBag.PrimaryRejectionReasonsID = new SelectList(db.PrimaryRejectionReasons, "ID", "Reason", ccaUpload.PrimaryRejectionReasonsID);
            ViewBag.ProviderID = new SelectList(db.Providers, "ID", "Name", ccaUpload.ProviderID);
            ViewBag.ProviderRejectionReasonsID = new SelectList(db.ProviderRejectionReasons, "ID", "Reason", ccaUpload.ProviderRejectionReasonsID);
            ViewBag.SessionID = new SelectList(db.Session, "ID", "Name", ccaUpload.SessionID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "UserId", ccaUpload.StudentID);
            return View(ccaUpload);
        }

        // GET: CcaUploads/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CcaUpload ccaUpload = await db.CcaUpload.FindAsync(id);
            if (ccaUpload == null)
            {
                return HttpNotFound();
            }
            return View(ccaUpload);
        }

        // POST: CcaUploads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CcaUpload ccaUpload = await db.CcaUpload.FindAsync(id);
            db.CCAs.Remove(ccaUpload);
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

        /// <summary>
        /// This method is the get portion of the process to upload Excel files to import CCAs.  It will create records in lookup tables when appropriate.
        /// It will fail on lines that have mandatory data that is missing or corrupt. It will not upload records that are withdrawn, rejected, or closed.
        /// Mandatory fields are:
        /// -SSID
        /// -SchoolID
        /// -Course Name
        /// -CategoryID
        /// -Provider
        /// -Status
        /// -Credit
        /// -Session
        /// -Provider
        /// -Counselor
        /// -Parent information
        /// -Primary LEA and SchoolId
        /// -Fiscal Year
        /// 
        /// (Additional information if student does not already exist in our system.)
        /// -Student Email 
        /// -Birthdate
        /// -Student First Name
        /// -Student Last Name
        /// 
        /// Other fields are set to null or specified in the spreadsheet.
        /// </summary>
        /// <returns></returns>
        public ActionResult BulkUpload()
        {
            return View(new BulkUploadViewModel());
        }


        [HttpPost]
        public async Task<ActionResult> BulkUpload(BulkUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int errorCount = 0;
            List<String> errorList = new List<String>();
            try
            {
                using (var dataTable = GetDataFromExcel(model))
                {
                    // Foreach record
                    foreach (DataRow row in dataTable.Rows)
                    {
                        try
                        {
                            var status = row["Completion Status"].ToString().ToLower();

                            //Do not import Withdrawn, rejected, or closed records.
                            if (!(status.Contains("withdrawn") || status.Contains("rejected") || status.Contains("closed")))
                            {
                                // Populate fields that are imported directly to database
                                var ccaLoad = BuildCca(row);

                                // Populate fields that are references to other tables.
                                await CheckTables(row, status, ccaLoad);



                                db.CcaUpload.Add(ccaLoad);
                                await db.SaveChangesAsync().ConfigureAwait(false);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException != null)
                                errorList.Add(String.Format("{0} {1} taking {2} *Errors*:{3},{4}", row["Student First Name"].ToString(), row["Student Last Name"].ToString(), row["Course"].ToString(), ex.Message, ex.InnerException.Message));
                            else
                                errorList.Add(String.Format("{0} {1} taking {2}  *Errors*:{3}", row["Student First Name"].ToString(), row["Student Last Name"].ToString(), row["Course"].ToString(), ex.Message));
                            errorCount++;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Unable to load file." + ex.Message;
                return View("Error");
            }

            ViewBag.Message = String.Format("Thanks for uploading the file.  There were {0} bad records that were not uploaded.", errorCount.ToString());
            ViewBag.ErrorList = errorList;
            return View("BulkUploadDetails");
        }

        /// <summary>
        /// This method takes the Excel row data and tries to look up existing entries in the tables.  If we are able we will create new
        /// entries when possible, otherwise the row will be rejected.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="status"></param>
        /// <param name="cca"></param>
        /// <returns></returns>
        private async Task CheckTables(DataRow row, string status, CcaUpload cca)
        {
            try
            {
                var student = await FindOrCreateStudent(row);
                cca.StudentID = student.ID;
                cca.EnrollmentLocationID = student.EnrollmentLocationID;
                cca.EnrollmentLocationSchoolNamesID = student.EnrollmentLocationSchoolNamesID;
                cca.UserId = student.UserId;

                var ccaStatus = await FindStatus(status);
                cca.CourseCompletionStatusID = ccaStatus.ID;

                // Homeschool are assigned Counselor ID=0
                var counselor = student.EnrollmentLocationID != GlobalVariables.HOMESCHOOLID ? await FindOrCreateCounselor(row, student) :
                    new Counselor() { ID = 0 };
                cca.CounselorID = counselor.ID;

                var provider = await FindProvider(row);
                cca.ProviderID = provider.ID;

                var session = await FindSession(row);
                cca.SessionID = session.ID;

                var category = await FindCategory(row);
                cca.CourseCategoryID = category.ID;

                var course = await FindCourse(row, cca);
                cca.OnlineCourseID = course.ID;

                var credit = await FindCredit(row);
                cca.CourseCreditID = credit.ID;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Populate available fields in a CCA from an Excel row formatted to be imported to our database.  The Column titles in
        /// row must match exactly for the fields to be imported correctly.
        /// TODO: Find way to import header and assign titles dynamically.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private CcaUpload BuildCca(DataRow row)
        {
            try
            {
                var cca = new CcaUpload();
                var submissionDate = row["Submission Date"].ToString().Trim();
                if (submissionDate != "")
                    cca.ApplicationSubmissionDate = Convert.ToDateTime(submissionDate);

                var cactusId = row["Teacher of Record CACTUS ID"].ToString().Trim();
                if (cactusId != "")
                    cca.TeacherCactusID = Convert.ToInt32(cactusId);
                cca.TeacherFirstName = row["Teacher of Record First Name"].ToString();
                cca.TeacherLastName = row["Teacher of Record Last Name"].ToString();

                var fee = row["COURSE FEE"].ToString().Trim();
                if (fee != "")
                    cca.CourseFee = Convert.ToDecimal(fee);

                // If fiscal year is missing we will not be able to look up Session.  Skip record. 

                var fiscalYearString = row["Fiscal Year"].ToString().Trim();
                var fiscalYear1 = fiscalYearString.Substring(0, 4);

                cca.FiscalYear = Convert.ToInt32(fiscalYear1) + 1;

                var startDate = row["Course Start Date"].ToString().Trim();
                if (startDate != "")
                    cca.CourseStartDate = Convert.ToDateTime(startDate);

                var completeDate = row["Date of Course Completion"].ToString().Trim();
                if (completeDate != "")
                    cca.CourseCompletionDate = Convert.ToDateTime(completeDate);

                var withdrawlDate = row["WITHDRAWAL (Date)"].ToString().Trim();
                if (withdrawlDate != "")
                    cca.WithdrawalDate = Convert.ToDateTime(withdrawlDate);

                var confirmDate = row["Date of Confirmation of Active Participation"].ToString().Trim();
                if (confirmDate != "")
                    cca.DateConfirmationActiveParticipation = Convert.ToDateTime(confirmDate);

                var continueDate = row["Date of Continuation of Active Participation"].ToString().Trim();
                if (continueDate != "")
                    cca.DateContinuationActiveParticipation = Convert.ToDateTime(continueDate);

                var budget = row["BUDGET (PRIMARY/PROVIDER)"].ToString().Trim();
                if (budget != "")
                    cca.BudgetPrimaryProvider = Convert.ToDecimal(budget);

                cca.SubmitterTypeID = FindOrCreateSubmitterType(row);

                var grade = row["Grade Level"].ToString().Trim();
                if (grade != "")
                    cca.StudentGradeLevel = Convert.ToInt32(grade);

                // Set all verification flags to true.
                cca.IsBusinessAdministratorAcceptRejectEnrollment = true;
                cca.IsCounselorSigned = true;
                cca.IsProviderAcceptsRejectsCourseRequest = true;
                cca.IsProviderEnrollmentVerified = true;
                cca.IsProviderSignature = true;
                cca.IsCourseConsistentWithStudentSEOP = true;
                cca.IsEnrollmentNoticeSent = true;
                return cca;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the submitter type.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private int FindOrCreateSubmitterType(DataRow row)
        {
            try
            {
                switch (row["Submitter Type"].ToString())
                {
                    case "Student/Parent or Guardian":
                        return (int)GlobalVariables.SubmitterTypeID.StudentParent;
                    case "Counselor":
                        return (int)GlobalVariables.SubmitterTypeID.Counselor;
                    case "Provider":
                        return (int)GlobalVariables.SubmitterTypeID.Provider;
                    default:
                        return 0;
                }
            }
            catch
            {
                throw new EvaluateException("Unable to evaluate submitter type");
            }
        }

        /// <summary>
        /// Attempts to read and and lookup the Completion status of the CCA.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private async Task<CourseCompletionStatus> FindStatus(string status)
        {
            try
            {
                var statusLookup = await db.CourseCompletionStatus.Where(m => m.Status.ToLower() == status.Trim().ToLower()).FirstOrDefaultAsync();
                if (statusLookup != null)
                    return statusLookup;
                else
                    throw new NullReferenceException("Error reading Status field.");
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to find Course Completion Status", ex);
            }
        }

        /// <summary>
        /// Attempts to read and lookup the credit value from the CourseCredits table.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private async Task<CourseCredit> FindCredit(DataRow row)
        {
            try
            {
                var credit = row["Credit"].ToString();
                var creditLookup = await db.CourseCredits.Where(m => m.Value.Trim().Contains(credit.Trim())).FirstOrDefaultAsync();
                if (creditLookup != null)
                    return creditLookup;
                else
                    throw new NullReferenceException("Error reading Credit field");
            }
            catch (Exception ex)
            {
                throw new Exception("Error assigning Course Credits.", ex);
            }

        }

        /// <summary>
        /// Finds course in Courses table.  Matches name.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="category"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        private async Task<OnlineCourse> FindCourse(DataRow row, CcaUpload cca)
        {
            try
            {
                OnlineCourse course;
                //var code = row["Code"].ToString(); && m.Code.Trim() == code.Trim()
                var courseLookup = await db.Courses.Where(n => n.CourseCategoryID == cca.CourseCategoryID && n.ProviderID == cca.ProviderID).ToListAsync();
                var code = row["Code"].ToString().Trim();
                var courses = courseLookup.Where(m => m.Code == code);
                if (courses.Count() == 0)
                {
                    course = await CreateCourse(row, cca);

                }
                //This else tries to match name for multiple returns for same code.  Decided to just take first instance since it is the same code.
                //else if(courses.Count() > 1)
                //{

                //    var courseName = row["Course"].ToString();
                //    var courseMatches = courses.Where(m => m.Name.Trim() == courseName.Trim());
                //    if (courseMatches == null)
                //    {
                //        var courseNameBreakdown = courseName.Split(':');
                //        var courseTitle = courseNameBreakdown.Count() > 1 ? courseNameBreakdown[1] : courseNameBreakdown[0];
                //        course = courses.Where(m => m.Name.Contains(courseTitle.Trim())).FirstOrDefault();
                //    }
                //    else // If courses not null match the first one.  (Code, course name, provider, and category, all match probably just Session)
                //    {
                //        course = courseMatches.First();
                //    }

                //}
                else
                {
                    //Set course to first code match for that provider.
                    course = courses.First();
                }


                if (course != null)
                    return course;
                else
                    throw new NullReferenceException("Course not found or created!");
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Course.", ex);
            }

        }

        private async Task<OnlineCourse> CreateCourse(DataRow row, CcaUpload cca)
        {
            try
            {
                OnlineCourse newCourse = new OnlineCourse();
                newCourse.Code = row["Code"].ToString().Trim();
                newCourse.CourseCategoryID = cca.CourseCategoryID;
                newCourse.ProviderID = cca.ProviderID;
                newCourse.SessionID = 0;
                newCourse.Name = row["Course"].ToString();
                newCourse.IsActive = false;
                newCourse.Credit = "1111";
                newCourse.Notes = "Added during bulk CCA upload.";

                db.Courses.Add(newCourse);
                await db.SaveChangesAsync().ConfigureAwait(false);

                return newCourse;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to write new Course to table. Error:" + ex.Message);
            }
        }

        /// <summary>
        /// Tries to match Category from row to category in table. If the category ID precedes the name we use it to look it up in table.
        /// Otherwise use the name.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private async Task<CourseCategory> FindCategory(DataRow row)
        {
            try
            {
                var category = row["Category"].ToString();
                CourseCategory categoryLookup;
                if (Char.IsDigit(category[0]))
                {
                    var categoryId = Convert.ToInt64(category[0].ToString().Trim());
                    categoryLookup = await db.CourseCategories.Where(m => m.ID == categoryId).FirstOrDefaultAsync();
                }
                else // In cases where category is specified by name not ID
                {
                    //var categoryBreakdown = category.Split();
                    var categoryName = category.Remove(0, category.IndexOf(' ') + 1).Trim();
                    categoryLookup = await db.CourseCategories.Where(m => m.Name == categoryName).FirstOrDefaultAsync();
                }

                if (categoryLookup != null)
                    return categoryLookup;
                else
                    throw new NullReferenceException("Unable to find Category" + category);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in assigning category", ex);
            }
        }

        /// <summary>
        /// Tries to find session course was taken.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private async Task<Session> FindSession(DataRow row)
        {
            try
            {
                var semester = row["Session"].ToString().Trim().Split();

                var yearString = row["Fiscal Year"].ToString().Trim();

                var year1 = yearString.Substring(0, 4);

                int yearBegin = Convert.ToInt32(year1);
                int yearEnd = yearBegin + 1;

                var year2 = yearEnd.ToString();

                var sessionName = semester[0] + " (" + year1 + "-" + year2 + ")";

                var session = await db.Session.Where(m => m.Name == sessionName).FirstOrDefaultAsync();

                if (session != null)
                    return session;
                else
                    throw new NullReferenceException("Unable to find Session");
            }
            catch (Exception ex)
            {
                throw new NullReferenceException("Error assigning Session.", ex);
            }
        }

        /// <summary>
        /// Tries to find Provider of course in CCA.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private async Task<Provider> FindProvider(DataRow row)
        {
            try
            {
                var provider = row["PROVIDER"].ToString().Split();
                var providerDistrict = provider[0].Trim();
                var providerLookup = providerDistrict != null ? await db.Providers.Where(m => m.DistrictNumber == providerDistrict).FirstOrDefaultAsync() : null;

                // If lookup by district fails try by name
                if (providerLookup == null)
                {
                    var providerName = provider[1].Trim().ToUpper();
                    providerLookup = providerName != null ? await db.Providers.Where(m => m.Name.ToUpper() == providerName).FirstOrDefaultAsync() : null;
                }
                if (provider != null)
                    return providerLookup;
                else
                    throw new NullReferenceException("Provider lookup from database failed.");
            }
            catch (Exception ex)
            {
                throw new NullReferenceException("Unable to find Provider.", ex);
            }
        }

        /// <summary>
        /// Attempts to find Counselor in table using email otherwise creates one.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        private async Task<Counselor> FindOrCreateCounselor(DataRow row, Student student)
        {
            try
            {
                var counselorEmail = row["Counselor Email"].ToString().ToLower().Trim();
                var counselorLookup = counselorEmail != null ? await db.Counselors.Where(m => m.Email.ToLower().Trim() == counselorEmail).FirstOrDefaultAsync() :
                    null;

                if (counselorLookup == null)
                {
                    counselorLookup = new Counselor();
                    counselorLookup.Email = counselorEmail;
                    counselorLookup.FirstName = row["Counselor First Name"].ToString().Trim();
                    counselorLookup.LastName = row["Counselor Last Name"].ToString().Trim();
                    counselorLookup.Phone = row["Counselor Telephone"].ToString().Trim();
                    var cactusId = row["Counselor Cactus ID"].ToString().Trim();
                    if (cactusId != "")
                    {
                        counselorLookup.CactusID = Convert.ToInt32(cactusId);
                    }

                    // Counselor must belong to same School as student
                    counselorLookup.EnrollmentLocationID = student.EnrollmentLocationID;
                    counselorLookup.EnrollmentLocationSchoolNameID = student.EnrollmentLocationSchoolNamesID;

                    db.Counselors.Add(counselorLookup);

                    await db.SaveChangesAsync().ConfigureAwait(false);
                }

                if (counselorLookup != null)
                    return counselorLookup;
                else
                    throw new NullReferenceException("Counselor creation failed.");
            }
            catch (Exception ex)
            {
                throw new NullReferenceException("Unable to assign Counselor.", ex);
            }
        }

        /// <summary>
        /// Try to match student in cca upload to record in table.  cca record must contain ssid.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private async Task<Student> FindOrCreateStudent(DataRow row)
        {
            try
            {
                Student student = new Student();

                var studentNumber = row["LEA Student Number"].ToString().Trim();
                if (studentNumber != "")
                {
                    student.StudentNumber = Convert.ToInt32(studentNumber);
                }

                student.SSID = row["SSID"].ToString();

                // Look for existing ssid
                var studentLookup = student.SSID != "" ? await db.Students.Where(m => m.SSID.Trim() != "No Records Found" && m.SSID.Trim() == student.SSID.Trim()).FirstOrDefaultAsync().ConfigureAwait(false) : null;

                // if student not found by ssid create a table entry
                if (studentLookup != null)
                {
                    student = studentLookup;
                }
                else
                {
                    //Populate enrollment information
                    await GetEnrollmentIDs(row, student);

                    //Lookup existing parent
                    await FindOrCreateParent(row, student);

                    GetStudentFieldsFromRow(row, student);

                    // Create ASP.Net User for this student
                    await CreateAspNetUser(student);

                    // add student to database
                    db.Students.Add(student);
                    await db.SaveChangesAsync().ConfigureAwait(false);
                }

                return student;
            }
            catch (Exception ex)
            {
                throw new NullReferenceException("Unable to find or create this student.", ex);
            }
        }

        private static void GetStudentFieldsFromRow(DataRow row, Student student)
        {
            var gradDate = row["Graduation Date"].ToString().Trim();
            if (gradDate != "")
            {
                student.GraduationDate = Convert.ToDateTime(gradDate);
            }

            student.SSID = row["SSID"].ToString();
            var dob = row["Birth Date"].ToString().Trim();
            if (dob != "")
                student.StudentDOB = Convert.ToDateTime(dob);

            student.StudentFirstName = row["Student First Name"].ToString();
            student.StudentLastName = row["Student Last Name"].ToString();
            student.StudentEmail = row["Email"].ToString();
            student.IsFeeWaived = row["Fee Waiver?"].ToString().ToLower().Equals("yes") ? true : false;
            student.IsEarlyGraduate = row["SEOP for Early Graduation?"].ToString().ToLower().Equals("yes") ? true : false;
            student.IsIEP = row["IEP?"].ToString().ToLower().Equals("yes") ? true : false;
            student.IsSection504 = row["504 Accommodation?"].ToString().ToLower().Equals("yes") ? true : false;
        }

        private async Task CreateAspNetUser(Student student)
        {
            try
            {
                RegisterViewModel user = new RegisterViewModel();

                var start = student.SSID.Length - 5;
                if (start > 0)
                {
                    var last4OfSsid = student.SSID.Substring(start, 4);
                    var firstName = student.StudentFirstName.Split(); // Use only 1 first name.
                    var lastName = student.StudentLastName.Replace(' ', '-'); //Replace spaces in lastname
                    user.Username = firstName[0] + lastName + last4OfSsid;
                }
                else
                {
                    throw new NullReferenceException("Invalid SSID Length. Please check SSID.");
                }

                if (student.StudentEmail != "")
                    user.Email = student.StudentEmail;
                else if (student.Parent.GuardianEmail != "")
                    user.Email = student.Parent.GuardianEmail;
                else
                    throw new NullReferenceException("Unable to find contact email");

                user.Password = "!Changeme1";
                student.UserId = await RegisterAspNetUser(user);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to create a user for the BulkUpload.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        private async Task<String> RegisterAspNetUser(RegisterViewModel model)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email, EmailConfirmed = true, IsSetup = true };
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return user.Id;
            }
            else
            {
                try
                {
                    user.Email = model.Username + "@gmail.com";
                    result = await userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return user.Id;
                    }
                    else
                        throw new Exception("Unable to resolve with new email.");
                }
                catch (Exception ex)
                {
                    throw new NullReferenceException("Error in creating User Account. Error:" + ex.Message);
                }
            }

        }

        private async Task FindOrCreateParent(DataRow row, Student student)
        {
            try
            {
                Parent parent = new Parent();
                parent.GuardianFirstName = row["Parent/Guardian First Name"].ToString();
                parent.GuardianLastName = row["Parent/Guardian Last Name"].ToString();
                parent.GuardianPhone1 = row["Parent Telephone"].ToString();
                parent.GuardianEmail = row["Parent/Guardian Email"].ToString();
                var studentEmail = row["Email"].ToString().Trim();
                // Deal with empty parent email field
                if (parent.GuardianEmail == "") //No parent email
                {
                    if (studentEmail != "")
                        parent.GuardianEmail = studentEmail;
                    else if (row["Parent Telephone"].ToString().Trim() != "")
                    {
                        var phone = row["Parent Telephone"].ToString().Trim();
                        var start = phone.Length - 5;
                        var last4OfPhone = student.SSID.Substring(start, 4);
                        var firstName = parent.GuardianFirstName.Replace(' ', '-');
                        var lastName = parent.GuardianLastName.Replace(' ', '-');
                        parent.GuardianEmail = firstName + lastName + last4OfPhone + "@gmail.com";
                    }
                    else
                    {
                        throw new NullReferenceException("No contact info for parent!");
                    }
                }

                var parentLookup = parent.GuardianEmail != "" ? await db.Parents.Where(m => m.GuardianEmail.ToLower().Trim() == parent.GuardianEmail.ToLower().Trim()).FirstOrDefaultAsync().ConfigureAwait(false) : null;

                if (parentLookup != null)
                {
                    student.ParentID = parentLookup.ID;
                    student.Parent = parentLookup;
                }
                else
                {
                    db.Parents.Add(parent);
                    await db.SaveChangesAsync().ConfigureAwait(false);
                    student.Parent = parent;
                }
            }
            catch (Exception ex)
            {
                throw new NullReferenceException("Unable to find or create Parent.", ex);
            }
        }


        /// <summary>
        /// Tries to find an existing student using the school.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="student"></param>
        /// <param name="studentList"></param>
        /// <returns></returns>
        private async Task StudentMatch(DataRow row, Student student, List<Student> studentList)
        {
            await GetEnrollmentIDs(row, student);

            var studentItem = studentList.Where(m => m.EnrollmentLocationID == student.EnrollmentLocationID).FirstOrDefault();
            if (studentItem != null)
            {
                student.ID = studentItem.ID;
            }
        }

        /// <summary>
        /// This method depends on the PRIMARY SCHOOL being set as the Cactus ID
        /// </summary>
        /// <param name="row"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        private async Task GetEnrollmentIDs(DataRow row, Student student)
        {
            CactusSchool school;
            var primaryName = row["PRIMARY LEA"].ToString().ToUpper().Trim();

            if (primaryName.Contains("HOME"))
            {
                student.EnrollmentLocationID = GlobalVariables.HOMESCHOOLID;
                student.EnrollmentLocationSchoolNamesID = GlobalVariables.HOMESCHOOLID;
            }
            else
            {
                // Try to look up School using name.
                //var schoolName = row["PRIMARY SCHOOL"].ToString().ToUpper().Trim();
                var schoolIdString = row["PRIMARY SCHOOL"].ToString().Trim();
                if (schoolIdString != "")
                {
                    var schoolId = Convert.ToInt64(schoolIdString);
                    school = schoolId != 0 ? await cactus.CactusSchools.Where(m => m.ID == schoolId).FirstOrDefaultAsync().ConfigureAwait(false) : null;

                    //school = schoolName != null ? await cactus.CactusSchools.Where(m => m.Name == schoolName.ToUpper().Trim()).FirstOrDefaultAsync().ConfigureAwait(false) : null;

                    if (school != null)
                    {
                        if (school.SchoolType == GlobalVariables.PRIVATESCHOOLTYPE)
                            student.EnrollmentLocationID = GlobalVariables.PRIVATESCHOOLID;
                        else
                            student.EnrollmentLocationID = school.District;

                        student.EnrollmentLocationSchoolNamesID = school.ID;
                    }
                    else
                    {
                        throw new NullReferenceException("Unable to Find School.");
                    }
                    //else // school lookup failed plan b.
                    //{

                    //    if (primaryName.Contains("PRIVATE"))
                    //    {
                    //        student.EnrollmentLocationID = GlobalVariables.PRIVATESCHOOLID;
                    //        student.SchoolOfRecord = schoolName != null ? schoolName : "UNKNOWN";
                    //    }
                    //    else
                    //    {
                    //        try
                    //        {
                    //            await FindSchool(student, primaryName, schoolName);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            throw new NullReferenceException("Unable to Find School. :" + ex.Message);
                    //        }
                    //    } // endif primaryName.Contains("PRIVATE")
                    //} // endif school != null
                }
            }// endif primaryName.Contains("HOME")
        }

        private async Task FindSchool(Student student, string primaryName, string schoolName)
        {
            var leaBreakdown = primaryName.Split();
            var leaCode = leaBreakdown[0];
            var leaName = leaBreakdown[1];
            var lea = await cactus.CactusInstitutions.Where(m => m.Code.Trim() == leaCode.Trim()).FirstOrDefaultAsync();
            if (lea == null && leaName != null)
            {
                lea = await cactus.CactusInstitutions.Where(m => m.Name.Trim() == leaName.Trim()).FirstOrDefaultAsync();
            }

            // Try to find school in district. Use first 2 terms in name.  If not found by then give up.
            if (schoolName != null && lea != null)
            {
                student.EnrollmentLocationID = lea.ID;
                var schoolNameBreakdown = schoolName.ToUpper().Split();

                var schools = await cactus.CactusSchools.Where(m => m.District == lea.ID).ToListAsync();
                if (schoolNameBreakdown.Count() > 1)
                {
                    var term1 = schoolNameBreakdown[0] + " " + schoolNameBreakdown[1];
                    var schoolList = schools.Where(m => m.Name.Contains(term1)).ToList();

                    if (schoolList.Count == 1)
                    {
                        student.EnrollmentLocationSchoolNamesID = schoolList[0].ID;
                    }
                    else if (schoolNameBreakdown[2] != null && schoolList.Count != 0)
                    {
                        var term2 = term1 + " " + schoolNameBreakdown[2];
                        var schoolList2 = schoolList.Where(m => m.Name.Contains(term2)).ToList();
                        if (schoolList2.Count == 1)
                        {
                            student.EnrollmentLocationSchoolNamesID = schoolList2[0].ID;
                        }
                    } // endif schoolList.Count == 0
                }// endif schoolNameBreakdown.Count() > 1
                else if (schoolNameBreakdown.Count() == 1 && schoolNameBreakdown[0] != "")
                {
                    var term1 = schoolNameBreakdown[0];
                    var schoolList = schools.Where(m => m.Name.Contains(term1)).ToList();

                    if (schoolList.Count == 1)
                    {
                        student.EnrollmentLocationSchoolNamesID = schoolList[0].ID;
                    }
                }

                // No match found throw exception.

                if (student.EnrollmentLocationSchoolNamesID == null)
                {
                    throw new NullReferenceException("Unable to find match for school.");
                }

            } // endif schoolName != null
        }

        /// <summary>
        /// 30 Mb file size limit by IIS upload.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static DataTable GetDataFromExcel(BulkUploadViewModel model)
        {
            try
            {
                using (var package = new ExcelPackage(model.File.InputStream))
                {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    DataTable tbl = new DataTable();
                    bool hasHeader = true;
                    foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                    {
                        tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                    }
                    var startRow = hasHeader ? 2 : 1;
                    for (var rowNum = startRow; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = worksheet.Cells[rowNum, 1, rowNum, worksheet.Dimension.End.Column];
                        var row = tbl.NewRow();
                        foreach (var cell in wsRow)
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                        tbl.Rows.Add(row);
                    }
                    return tbl;
                }
            }
            catch
            {
                throw new FormatException("Error reading Excel file.");
            }
        }


    }
}
