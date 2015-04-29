using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CcaRegistrationDf.Models;

namespace CcaRegistrationDf.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EnrollmentLocationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EnrollmentLocations
        public async Task<ActionResult> Index()
        {
            return View(await db.EnrollmentLocations.ToListAsync());
        }

        // GET: EnrollmentLocations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnrollmentLocation enrollmentLocation = await db.EnrollmentLocations.FindAsync(id);
            if (enrollmentLocation == null)
            {
                return HttpNotFound();
            }
            return View(enrollmentLocation);
        }

        // GET: EnrollmentLocations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EnrollmentLocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,DistrictId")] EnrollmentLocation enrollmentLocation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.EnrollmentLocations.Add(enrollmentLocation);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(enrollmentLocation);
        }

        // GET: EnrollmentLocations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnrollmentLocation enrollmentLocation = await db.EnrollmentLocations.FindAsync(id);
            if (enrollmentLocation == null)
            {
                return HttpNotFound();
            }
            return View(enrollmentLocation);
        }

        // POST: EnrollmentLocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,DistrictId")] EnrollmentLocation enrollmentLocation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(enrollmentLocation).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(enrollmentLocation);
        }

        // GET: EnrollmentLocations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnrollmentLocation enrollmentLocation = await db.EnrollmentLocations.FindAsync(id);
            if (enrollmentLocation == null)
            {
                return HttpNotFound();
            }
            return View(enrollmentLocation);
        }

        //// POST: EnrollmentLocations/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    EnrollmentLocation enrollmentLocation = await db.EnrollmentLocations.FindAsync(id);
        //    db.EnrollmentLocations.Remove(enrollmentLocation);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
