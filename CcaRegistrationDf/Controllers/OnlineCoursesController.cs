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
    public class OnlineCoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OnlineCourses
        public async Task<ActionResult> Index()
        {
            var courses = db.Courses.Include(o => o.CourseCategory).Include(o => o.Provider).Include(o => o.Session);
            return View(await courses.ToListAsync());
        }

        // GET: OnlineCourses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineCourse onlineCourse = await db.Courses.FindAsync(id);
            if (onlineCourse == null)
            {
                return HttpNotFound();
            }
            return View(onlineCourse);
        }

        // GET: OnlineCourses/Create
        public ActionResult Create()
        {
            ViewBag.CourseCategoryID = new SelectList(db.CourseCategories, "ID", "Name");
            ViewBag.ProviderID = new SelectList(db.Providers, "ID", "Name");
            ViewBag.SessionID = new SelectList(db.Session, "ID", "Name");
            return View();
        }

        // POST: OnlineCourses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,Credit,Code,IsActive,Notes,CourseCategoryID,ProviderID,SessionID")] OnlineCourse onlineCourse)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(onlineCourse);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseCategoryID = new SelectList(db.CourseCategories, "ID", "Name", onlineCourse.CourseCategoryID);
            ViewBag.ProviderID = new SelectList(db.Providers, "ID", "Name", onlineCourse.ProviderID);
            ViewBag.SessionID = new SelectList(db.Session, "ID", "Name", onlineCourse.SessionID);
            return View(onlineCourse);
        }

        // GET: OnlineCourses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineCourse onlineCourse = await db.Courses.FindAsync(id);
            if (onlineCourse == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseCategoryID = new SelectList(db.CourseCategories, "ID", "Name", onlineCourse.CourseCategoryID);
            ViewBag.ProviderID = new SelectList(db.Providers, "ID", "Name", onlineCourse.ProviderID);
            ViewBag.SessionID = new SelectList(db.Session, "ID", "Name", onlineCourse.SessionID);
            return View(onlineCourse);
        }

        // POST: OnlineCourses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,Credit,Code,IsActive,Notes,CourseCategoryID,ProviderID,SessionID")] OnlineCourse onlineCourse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(onlineCourse).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseCategoryID = new SelectList(db.CourseCategories, "ID", "Name", onlineCourse.CourseCategoryID);
            ViewBag.ProviderID = new SelectList(db.Providers, "ID", "Name", onlineCourse.ProviderID);
            ViewBag.SessionID = new SelectList(db.Session, "ID", "Name", onlineCourse.SessionID);
            return View(onlineCourse);
        }

        // GET: OnlineCourses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineCourse onlineCourse = await db.Courses.FindAsync(id);
            if (onlineCourse == null)
            {
                return HttpNotFound();
            }
            return View(onlineCourse);
        }

        // POST: OnlineCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OnlineCourse onlineCourse = await db.Courses.FindAsync(id);
            db.Courses.Remove(onlineCourse);
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
