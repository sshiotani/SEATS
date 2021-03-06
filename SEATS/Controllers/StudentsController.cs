﻿using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;

using SEATS.Models;

using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System;
using System.Web;
using AutoMapper;
using SEATS.Models.Services;
using SEATS.Models.Interfaces;


namespace SEATS.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {

        private const short MAXAGE = 17; // Not currently enrolled students must be under 18
                                         // Sets custom CactusInstitution IDs for EnrollmentLocation list.

        private ApplicationDbContext db = new ApplicationDbContext();
        //private SeatsContext db { get; set; }
        private SEATSEntities cactus = new SEATSEntities();
        private ISsidFindingService ssidFindingService;

        public StudentsController()
        {
            this.ssidFindingService = new SsidFindingService();
        }

        // GET: Students


        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();

            if (User.IsInRole("Admin"))
            {
                var students = await db.Students.ToListAsync().ConfigureAwait(false);
                return View(students);
            }
            else if (User.IsInRole("Primary"))
            {
                var schoolId = await db.PrimaryUsers.Where(m => m.UserId == userId).Select(m => m.EnrollmentLocationSchoolNameID).FirstOrDefaultAsync().ConfigureAwait(false);
                var primaryStudents = await db.Students.Where(m => m.EnrollmentLocationSchoolNamesID == schoolId).ToListAsync().ConfigureAwait(false);
                return View(primaryStudents);
            }
            else if (User.IsInRole("Counselor"))
            {
                var schoolId = await db.Counselors.Where(m => m.UserId == userId).Select(m => m.EnrollmentLocationSchoolNameID).FirstOrDefaultAsync().ConfigureAwait(false);
                var primaryStudents = await db.Students.Where(m => m.EnrollmentLocationSchoolNamesID == schoolId).ToListAsync().ConfigureAwait(false);
                return View(primaryStudents);
            }

            var student = await db.Students.FirstOrDefaultAsync(u => u.UserId == userId).ConfigureAwait(false);

            if (student != null)
            {
                return View(student);
            }

            return RedirectToAction("Create");
        }

        // GET: Students/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.FindAsync(id).ConfigureAwait(false);

            if (student == null)
            {
                return HttpNotFound();
            }

            if (student.EnrollmentLocationID == GlobalVariables.HOMESCHOOLID)
            {
                ViewBag.Lea = "HOME SCHOOL";
                ViewBag.School = "HOME SCHOOL";

            }
            else if (student.EnrollmentLocationID == GlobalVariables.PRIVATESCHOOLID)
            {
                ViewBag.Lea = "PRIVATE SCHOOL";
                ViewBag.School = student.SchoolOfRecord;
            }
            else
            {
                if (student.EnrollmentLocationID != null)
                    ViewBag.Lea = await cactus.CactusInstitutions.Where(m => m.ID == student.EnrollmentLocationID).Select(m => m.Name).FirstOrDefaultAsync();
                else
                    ViewBag.Lea = "UNKNOWN";

                if (student.EnrollmentLocationSchoolNamesID != null)
                    ViewBag.School = await cactus.CactusSchools.Where(m => m.ID == student.EnrollmentLocationSchoolNamesID).Select(m => m.Name).FirstOrDefaultAsync();
                else
                    ViewBag.School = "UNKNOWN";
            }


            return View(student);
        }

        // GET: Students/Create
        public async Task<ActionResult> Create()
        {
            try
            {


                // Check to see if this information already exists.  If not then create one.
                var userIdentity = User.Identity.GetUserId();
                var student = await db.Students.FirstOrDefaultAsync(u => u.UserId == userIdentity).ConfigureAwait(false);
                if (student != null)
                {
                    //Delete entry in table make student re-enter data.
                    db.Students.Remove(student);
                }

                StudentViewModel model = new StudentViewModel();
                model = await GetClientSelectLists(model).ConfigureAwait(false); // Create SelectLists for Enrollment and Credit Exceptions
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View("Error");
            }

        }

        private async Task<StudentViewModel> GetClientSelectLists(StudentViewModel model)
        {
            try
            {
                //Look up Lists of Leas
                model.EnrollmentLocation = await GetEnrollmentLocations().ConfigureAwait(false);

                //Look up Lists of Schools
                model.EnrollmentLocationSchoolNames = GetSchoolNames();

                return model;
            }
            catch
            {
                throw;
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
                var leaList = await cactus.CactusInstitutions.OrderBy(m => m.Name).ToListAsync().ConfigureAwait(false);

                leaList.Insert(0, new CactusInstitution() { Name = "HOME SCHOOL", ID = GlobalVariables.HOMESCHOOLID });
                leaList.Insert(0, new CactusInstitution() { Name = "PRIVATE SCHOOL", ID = GlobalVariables.PRIVATESCHOOLID });

                var locations = leaList.Select(f => new SelectListItem
                {
                    Value = f.ID.ToString(),
                    Text = f.Name
                });

                return locations;
            }
            catch
            {
                throw;

            }
        }

        private List<SelectListItem> GetSchoolNames()
        {
            return new List<SelectListItem>();
        }

        /// <summary>
        /// Method gets lea names dynamically called from view. Matches the district id with
        /// the selected district id.  Removes null and list entries with DISTRICT selections.
        /// </summary>
        /// <param name="district"></param>
        /// <returns>json list with selectlist</returns>
        public async Task<JsonResult> GetSchoolNames(int district)
        {
            try
            {

                IEnumerable<SelectListItem> schoolNameList;
                var schoolList = await cactus.CactusSchools.OrderByDescending(m => m.ID).ToListAsync().ConfigureAwait(false);
                schoolList.RemoveAll(m => m.Name == null);
                var distinctSchoolList = schoolList.GroupBy(x => x.Name).Select(group => group.First());

                schoolNameList = distinctSchoolList.Where(m => m.District == district && !m.Name.ToLower().Contains("district")).OrderBy(m => m.Name).Distinct().Select(f => new SelectListItem
                {
                    Value = f.ID.ToString(),
                    Text = f.Name
                });

                return Json(new SelectList(schoolNameList, "Value", "Text"));

            }
            catch
            {
                throw new HttpException(500, "Error processing request.");

            }

        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,StudentFirstName,StudentLastName,SSID,StudentDOB,StudentEmail,EnrollmentLocationID,EnrollmentLocationSchoolNamesID,GraduationDate,HasExcessiveFED,ExcessiveFEDExplanation,ExcessiveFEDReasonID,IsEarlyGraduate,IsFeeWaived,IsIEP,IsPrimaryEnrollmentVerified,IsSection504,HasHomeSchoolRelease,SchoolOfRecord,Comments,StudentNumber")] StudentViewModel studentVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Map studentViewModel to student and assign values
                    Mapper.CreateMap<StudentViewModel, Student>();
                    Student student = Mapper.Map<StudentViewModel, Student>(studentVm);

                    //Age check for not enrolled applicants

                    if ((studentVm.EnrollmentLocationID == 1 || studentVm.EnrollmentLocationID == 2) && AgeCheck(studentVm.StudentDOB))
                    {
                        ViewBag.Message = "Unable to process application. Error: Applicant too old";
                        return View("Error");
                    }

                    // Find SSID using ssidFindingService
                    student.SSID = await GetSSID(studentVm);

                    var duplicate = await CheckSSID(student.SSID);

                    if (duplicate)
                    {
                        ViewBag.Message = "A student with this SSID already exists in our system.  Please contact us.";
                        return View("Error");
                    }

                    student.UserId = User.Identity.GetUserId();

                    db.Students.Add(student);

                    var count = await db.SaveChangesAsync().ConfigureAwait(false);

                    //if (count == 0) // Set account setup to true if successfully added
                    //{
                    //    ViewBag.Message = "Unable to save student!";
                    //    return View("Error");
                    //}

                    // Check for parent association
                    if (student.ParentID == null || student.ParentID == 0)
                    {
                        return RedirectToAction("Create", "Parents");
                    }
                    else
                    {
                        // In the case where the student filled out the parent information but the Setup status was not saved.
                        var user = db.Users.Find(student.UserId);
                        user.IsSetup = true;
                        await db.SaveChangesAsync().ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Unable to create new student account! Error: " + ex.Message;
                    return View("Error");
                }
            }

            studentVm.EnrollmentLocationID = 0;
            return View(await GetClientSelectLists(studentVm).ConfigureAwait(false));
        }

        private async Task<bool> CheckSSID(string ssid)
        {
            if (!(ssid.Contains("No") || ssid.Contains("Multiple") || ssid == null))
            {
                var check = await db.Students.FirstOrDefaultAsync(m => m.SSID == ssid);
                if (check != null)
                    return true;
                else
                    return false;
            }

            return false;
        }


        /// <summary>
        /// Checks age for not currently enrolled applicants if age is over 17 not eligible.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private bool AgeCheck(DateTime? dateTime)
        {

            DateTime today = DateTime.Today;
            DateTime birthDate = dateTime ?? DateTime.Today;

            if (dateTime != today)
            {
                int age = today.Year - birthDate.Year;

                if (today < birthDate.AddYears(age)) age--;

                if (age > MAXAGE)
                    return true;
                else
                    return false;
            }

            throw new NullReferenceException();
        }


        /// <summary>
        /// Gets SSID from SSID Server
        /// </summary>
        /// <param name="studentVm"></param>
        /// <returns></returns>
        public async Task<string> GetSSID(StudentViewModel studentVm)
        {
            var result = await ssidFindingService.GetSsid(studentVm);

            if (result != null)
            {
                return result;
            }

            throw new NullReferenceException();
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.FindAsync(id).ConfigureAwait(false);

            Mapper.CreateMap<Student, StudentViewModel>();
            var model = Mapper.Map<Student, StudentViewModel>(student);

            if (student == null)
            {
                return HttpNotFound();
            }

            var leaList = await cactus.CactusInstitutions.OrderBy(m => m.Name).ToListAsync().ConfigureAwait(false);

            leaList.Insert(0, new CactusInstitution() { Name = "HOME SCHOOL", ID = GlobalVariables.HOMESCHOOLID });
            leaList.Insert(0, new CactusInstitution() { Name = "PRIVATE SCHOOL", ID = GlobalVariables.PRIVATESCHOOLID });

            ViewBag.EnrollmentLocationID = new SelectList(leaList, "ID", "Name", model.EnrollmentLocationID);
            ViewBag.EnrollmentLocationSchoolNamesID = new SelectList(cactus.CactusSchools.Where(m => m.District == model.EnrollmentLocationID), "ID", "Name", model.EnrollmentLocationSchoolNamesID);

            return View(model);

        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Did not bind properties since it is an admin only method.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<StudentViewModel, Student>();
                Student student = await db.Students.FindAsync(model.ID).ConfigureAwait(false);
                Mapper.Map<StudentViewModel, Student>(model, student);

                db.Entry(student).State = EntityState.Modified;
                await db.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction("Index");
            }

            var leaList = await cactus.CactusInstitutions.OrderBy(m => m.Name).ToListAsync().ConfigureAwait(false);

            leaList.Insert(0, new CactusInstitution() { Name = "HOME SCHOOL", ID = GlobalVariables.HOMESCHOOLID });
            leaList.Insert(0, new CactusInstitution() { Name = "PRIVATE SCHOOL", ID = GlobalVariables.PRIVATESCHOOLID });

            ViewBag.EnrollmentLocationID = new SelectList(leaList, "ID", "Name", model.EnrollmentLocationID);
            ViewBag.EnrollmentLocationSchoolNamesID = new SelectList(cactus.CactusSchools.Where(m => m.District == model.EnrollmentLocationID), "ID", "Name", model.EnrollmentLocationSchoolNamesID);
            return View(model);
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.FindAsync(id).ConfigureAwait(false);
            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditAdmin([Bind(Include = "ID,StudentFirstName,StudentLastName,SSID,StudentDOB,StudentEmail,EnrollmentLocationID,GraduationDate,HasExcessiveFED,ExcessiveFEDExplanation,ExcessiveFEDReasonID,IsEarlyGraduate,IsFeeWaived,IsIEP,IsPrimaryEnrollmentVerified,IsSection504,HasHomeSchoolRelease,SchoolOfRecord")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(student);
        }


        // GET: Students/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.FindAsync(id).ConfigureAwait(false);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Student student = await db.Students.FindAsync(id).ConfigureAwait(false);
            db.Students.Remove(student);
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
