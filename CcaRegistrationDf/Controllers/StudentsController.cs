using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;

using CcaRegistrationDf.Models;

using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System;
using System.Web;
using AutoMapper;

namespace CcaRegistrationDf.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {      

      private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Students
        

        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();

            if (User.IsInRole("Admin"))
            {
                var students = await db.Students.ToListAsync();
                return View(students);
            }
            else if (User.IsInRole("Primary"))
            {
                var schoolId = await db.PrimaryUsers.Where(m => m.UserId == userId).Select(m => m.EnrollmentLocationSchoolNameID).FirstOrDefaultAsync();
                var primaryStudents = await db.Students.Where(m => m.EnrollmentLocationSchoolNamesID == schoolId).ToListAsync();
                return View(primaryStudents);
            }
            else if(User.IsInRole("Counselor"))
            {
                var schoolId = await db.Counselors.Where(m => m.UserId == userId).Select(m => m.EnrollmentLocationSchoolNameID).FirstOrDefaultAsync();
                var primaryStudents = await db.Students.Where(m => m.EnrollmentLocationSchoolNamesID == schoolId).ToListAsync();
                return View(primaryStudents);
            }

           
            var student = await db.Students.Where(u => u.UserId == userId).FirstOrDefaultAsync();

            if (student != null)
            {
                return View(student);
            }

            return RedirectToAction("Create");
        }

        // GET: Students/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public async Task<ActionResult> Create()
        {
            // Check to see if this information already parentID.  If not then create one.
            var UserIdentity = User.Identity.GetUserId();
            var student = await db.Students.Where(u => u.UserId == UserIdentity).FirstOrDefaultAsync();
            if (student == null)
            {
                var model = new StudentViewModel();
                try
                {
                    model = await GetClientSelectLists(model); // Create SelectLists for Enrollment and Credit Exceptions
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View("Error");
                }

                return View(model);
            }

            ModelState.AddModelError("", "You have already filled out this information.  If you need to make changes edit your account.");
            return View("Index");

        }

        private async Task<StudentViewModel> GetClientSelectLists(StudentViewModel model)
        {
            try
            {
                //Look up Lists of Leas
                model.EnrollmentLocation = await GetEnrollmentLocations();

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
                using (SEATSEntities1 leas = new SEATSEntities1())
                {
                    var leaList = await leas.CactusInstitutions.OrderBy(m => m.Name).ToListAsync();

                    leaList.Insert(0, new CactusInstitution() { Name = "HOME SCHOOL", ID = 1});
                    leaList.Insert(0, new CactusInstitution() { Name = "PRIVATE SCHOOL", ID = 2});

                    var locations = leaList.Select(f => new SelectListItem
                    {
                        Value = f.ID.ToString(),
                        Text = f.Name
                    });

                    return locations;
                }
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
        /// Method gets school names dynamically called from view. Matches the district id with
        /// the selected district id.  Removes null and list entries with DISTRICT selections.
        /// </summary>
        /// <param name="district"></param>
        /// <returns>json list with selectlist</returns>
        public async Task<JsonResult> GetSchoolNames(decimal district)
        {
            try
            {
                using (SEATSEntities1 schools = new SEATSEntities1())
                {
                    IEnumerable<SelectListItem> schoolNameList;
                    var schoolList = await schools.CactusSchools.ToListAsync();
                    schoolList.RemoveAll(m => m.Name == null);
                    schoolNameList = schoolList.Where(m => m.District == district && !m.Name.ToLower().Contains("district")).OrderBy(m => m.Name).Distinct().Select(f => new SelectListItem
                    {
                        Value = f.ID.ToString(),
                        Text = f.Name
                    });

                    return Json(new SelectList(schoolNameList, "Value", "Text"));
                }
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
        public async Task<ActionResult> Create([Bind(Include = "ID,StudentFirstName,StudentLastName,SSID,StudentDOB,StudentEmail,EnrollmentLocationID,EnrollmentLocationSchoolNamesID,GraduationDate,HasExcessiveFED,ExcessiveFEDExplanation,ExcessiveFEDReasonID,IsEarlyGraduate,IsFeeWaived,IsIEP,IsPrimaryEnrollmentVerified,IsSection504,HasHomeSchoolRelease,SchoolOfRecord,Comments")] StudentViewModel studentVm)
        {
            if (ModelState.IsValid)
            {
                //Map studentViewModel to student and assign values
                Mapper.CreateMap<StudentViewModel, Student>();
                Student student = Mapper.Map<StudentViewModel, Student>(studentVm);

               

                student.UserId = User.Identity.GetUserId();

                db.Students.Add(student);
                
                var count = await db.SaveChangesAsync();

                if (count != 0) // Set account setup to true if successfully added
                {
                    var user = await db.Users.Where(m => m.Id == student.UserId).FirstOrDefaultAsync();
                    user.IsSetup = true;
                    await db.SaveChangesAsync();
                }
                else
                {
                    ViewBag.Message = "Unable to save student!";
                    return View("Error");
                }

                
                return RedirectToAction("Index","Parents");
            }

            studentVm.EnrollmentLocationID = 0;
            return View(await GetClientSelectLists(studentVm));
        }

        // GET: Students/Edit/5
      
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.FindAsync(id);
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
        public async Task<ActionResult> Edit([Bind(Include = "ID,StudentFirstName,StudentLastName,SSID,StudentDOB,StudentEmail,EnrollmentLocationID,GraduationDate,HasExcessiveFED,ExcessiveFEDExplanation,ExcessiveFEDReasonID,IsEarlyGraduate,IsFeeWaived,IsIEP,IsPrimaryEnrollmentVerified,IsSection504,HasHomeSchoolRelease,SchoolOfRecord,StudentBudgetID")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(student);
        }
        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.FindAsync(id);
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
        public async Task<ActionResult> EditAdmin([Bind(Include = "ID,StudentFirstName,StudentLastName,SSID,StudentDOB,StudentEmail,EnrollmentLocationID,GraduationDate,HasExcessiveFED,ExcessiveFEDExplanation,ExcessiveFEDReasonID,IsEarlyGraduate,IsFeeWaived,IsIEP,IsPrimaryEnrollmentVerified,IsSection504,HasHomeSchoolRelease,SchoolOfRecord,StudentBudgetID")] Student student)
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
            Student student = await db.Students.FindAsync(id);
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
            Student student = await db.Students.FindAsync(id);
            db.Students.Remove(student);
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
