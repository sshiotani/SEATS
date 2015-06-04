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
    [Authorize(Roles="Admin")]
    public class CourseFeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CourseFees
        public async Task<ActionResult> Index()
        {
            return View(await db.CourseFees.ToListAsync().ConfigureAwait(false));
        }

        // GET: CourseFees/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseFee courseFee = await db.CourseFees.FindAsync(id).ConfigureAwait(false);
            if (courseFee == null)
            {
                return HttpNotFound();
            }
            return View(courseFee);
        }

        // GET: CourseFees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseFees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Fee")] CourseFee courseFee)
        {
            if (ModelState.IsValid)
            {
                db.CourseFees.Add(courseFee);
                await db.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction("Index");
            }

            return View(courseFee);
        }

        // GET: CourseFees/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseFee courseFee = await db.CourseFees.FindAsync(id).ConfigureAwait(false);
            if (courseFee == null)
            {
                return HttpNotFound();
            }
            return View(courseFee);
        }

        // POST: CourseFees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Fee")] CourseFee courseFee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseFee).State = EntityState.Modified;
                await db.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction("Index");
            }
            return View(courseFee);
        }

        // GET: CourseFees/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Message = "Delete disabled!";
            return View("Error");
        }

        //public async Task<ActionResult> Delete(int? id)
        //{

        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CourseFee courseFee = await db.CourseFees.FindAsync(id);
        //    if (courseFee == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(courseFee);
        //}

        // POST: CourseFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CourseFee courseFee = await db.CourseFees.FindAsync(id).ConfigureAwait(false);
            db.CourseFees.Remove(courseFee);
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
