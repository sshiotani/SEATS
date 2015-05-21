using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AutoMapper;
using CcaRegistrationDf.Models;

namespace CcaRegistrationDf.Controllers
{
    [Authorize]
    public class CounselorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CactusEntities cactusDb = new CactusEntities();

        // GET: Counselors
        public async Task<ActionResult> Index()
        {
            return View(await db.Counselors.ToListAsync());
        }

        // GET: Counselors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Counselor counselor = await db.Counselors.FindAsync(id);
            if (counselor == null)
            {
                return HttpNotFound();
            }
            return View(counselor);
        }

      
        // GET: Counselors/Create
        public async Task<ActionResult> Create()
        {
            var leas = await cactusDb.CactusInstitutions.ToListAsync();

            leas.Insert(0, new CactusInstitution() { Code = "", Name = "District", DistrictID = 0.0M });

            ViewBag.EnrollmentLocationID = new SelectList(leas, "DistrictID", "Name");

            ViewBag.EnrollmentLocationSchoolNameID = new List<SelectListItem>();
            ViewBag.CounselorID = new List<SelectListItem>();

            return View();
        }

        // POST: Counselors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Email, FirstName, LastName, Phone, CactusID, EnrollmentLocationSchoolNameID, School, CounselorID")] CounselorViewModel counselorVm)
        {
            if (ModelState.IsValid)
            {
                var counselor = await db.Counselors.Where(c => c.ID == counselorVm.CounselorID).FirstOrDefaultAsync();
                Mapper.CreateMap<CounselorViewModel, Counselor>();

                counselor = Mapper.Map<CounselorViewModel, Counselor>(counselorVm);

                if (counselorVm.CounselorID == 0)
                {
                    db.Counselors.Add(counselor);
                }

                counselor.SchoolID = counselorVm.EnrollmentLocationSchoolNameID;
                counselor.UserId = User.Identity.GetUserId();

                var count = await db.SaveChangesAsync();

                if (count != 0) // Set account setup to true if successfully added
                {
                    var user = await db.Users.Where(m => m.Id == counselor.UserId).FirstOrDefaultAsync();
                    user.IsSetup = true;
                    await db.SaveChangesAsync();
                }
                else
                {
                    ViewBag.Message = "Unable to save Primary User!";
                    return View("Error");
                }

                TempData["UserType"] = "Primary School User";

                return RedirectToAction("EmailAdminToConfirm","Account");
            }

            var leas = await cactusDb.CactusInstitutions.ToListAsync();

            leas.Insert(0, new CactusInstitution() { Code = "", Name = "District", DistrictID = 0.0M });

            ViewBag.EnrollmentLocationID = new SelectList(leas, "DistrictID", "Name");
            ViewBag.EnrollmentLocationSchoolNameID = new List<SelectListItem>();
            ViewBag.CounselorID = new List<SelectListItem>();
            return View(counselorVm);
        }


        public JsonResult GetCounselors(int schoolId)
        {
            var counselors =  db.Counselors.Where(m => m.SchoolID == schoolId).Select(f => new SelectListItem
            {
                Value = f.ID.ToString(),
                Text = f.FirstName + " " + f.LastName
            });

            // Add a item to add new counselor to list.

            var counselorList = counselors.AsEnumerable().Concat(new[] {new SelectListItem
                    {
                        Value = "0",
                        Text = "Counselor Not Listed."
                    }
                    });

            return Json( new SelectList(counselorList,"Value","Text") );
        }

        public async Task<JsonResult> GetCounselorInformation(int counselorId)
        {
            var counselor = await db.Counselors.Where(c => c.ID == counselorId).FirstOrDefaultAsync().ConfigureAwait(false);

            return Json(counselor);
        }

        // GET: Counselors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Counselor counselor = await db.Counselors.FindAsync(id);
            if (counselor == null)
            {
                return HttpNotFound();
            }
            return View(counselor);
        }

        // POST: Counselors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserId,PersonID,CactusID,Email,FirstName,LastName,Phone,School,SchoolID")] Counselor counselor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(counselor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(counselor);
        }

        // GET: Counselors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Counselor counselor = await db.Counselors.FindAsync(id);
            if (counselor == null)
            {
                return HttpNotFound();
            }
            return View(counselor);
        }

        // POST: Counselors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Counselor counselor = await db.Counselors.FindAsync(id);
            db.Counselors.Remove(counselor);
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
