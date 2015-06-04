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
    public class CourseCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CourseCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.CourseCategories.ToListAsync().ConfigureAwait(false));
        }

        // GET: CourseCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCategory courseCategory = await db.CourseCategories.FindAsync(id).ConfigureAwait(false);
            if (courseCategory == null)
            {
                return HttpNotFound();
            }
            return View(courseCategory);
        }

        // GET: CourseCategories/Create
        public ActionResult Create()
        {
            ViewBag.CourseFeeID = new SelectList(db.CourseFees, "ID", "Fee");
            return View();
        }

        // POST: CourseCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,IsActive,CourseFeeID")] CourseCategory courseCategory)
        {
            if (ModelState.IsValid)
            {
                db.CourseCategories.Add(courseCategory);
                await db.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction("Index");
            }

            ViewBag.CourseFeeID = new SelectList(db.CourseFees, "ID", "Fee");
            return View(courseCategory);
        }

        // GET: CourseCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCategory courseCategory = await db.CourseCategories.FindAsync(id).ConfigureAwait(false);
            if (courseCategory == null)
            {
                return HttpNotFound();
            }

            ViewBag.CourseFeeID = new SelectList(db.CourseFees, "ID", "Fee");
            return View(courseCategory);
        }

        // POST: CourseCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CourseFeeID,Name,IsActive")] CourseCategory courseCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseFeeID = new SelectList(db.CourseFees, "ID", "Fee");
            return View(courseCategory);
        }

        // GET: CourseCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Message = "Delete disabled for Categories! Disable if no longer needed.";
            return View("Error");
        }


        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CourseCategory courseCategory = await db.CourseCategories.FindAsync(id);
        //    if (courseCategory == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(courseCategory);
        //}

        // POST: CourseCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CourseCategory courseCategory = await db.CourseCategories.FindAsync(id).ConfigureAwait(false);
            db.CourseCategories.Remove(courseCategory);
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
