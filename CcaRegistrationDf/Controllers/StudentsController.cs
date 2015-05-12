using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using CcaRegistrationDf.DAL;
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
        private readonly string[] FEDOPTIONS =  {
        "Allowed by College and Career Ready Plan (SEOP or CCRP) providing for Early Graduation",
        "Allowed by school district or charter school board policy (check with your school district office)" };

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Students
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var students = await db.Students.ToListAsync();
                return View(students);
            }

            var UserIdentity = User.Identity.GetUserId();
            var student = await db.Students.Where(u => u.UserId == UserIdentity).ToListAsync();

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

                // List of Excessive credit reasons
                model.ExcessiveFEDReasonList = GetFEDReasonList();

                return model;
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
                        Value = Decimal.ToInt32(f.DistrictID).ToString(),
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

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,StudentFirstName,StudentLastName,SSID,StudentDOB,StudentEmail,EnrollmentLocationID,EnrollmentLocationSchoolNamesID,GraduationDate,HasExcessiveFED,ExcessiveFEDExplanation,ExcessiveFEDReasonCode,IsEarlyGraduate,IsFeeWaived,IsIEP,IsPrimaryEnrollmentVerified,IsSection504,HasHomeSchoolRelease,SchoolOfRecord,Comments")] StudentViewModel studentVm)
        {
            if (ModelState.IsValid)
            {
                //Map studentViewModel to student and assign values
                Mapper.CreateMap<StudentViewModel, Student>();
                Student student = Mapper.Map<StudentViewModel, Student>(studentVm);

                student.UserId = User.Identity.GetUserId();
                db.Students.Add(student);
                await db.SaveChangesAsync();
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
        public async Task<ActionResult> Edit([Bind(Include = "ID,StudentFirstName,StudentLastName,SSID,StudentDOB,StudentEmail,EnrollmentLocationID,GraduationDate,HasExcessiveFED,ExcessiveFEDExplanation,ExcessiveFEDReasonCode,IsEarlyGraduate,IsFeeWaived,IsIEP,IsPrimaryEnrollmentVerified,IsSection504,HasHomeSchoolRelease,SchoolOfRecord,StudentBudgetID")] Student student)
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
