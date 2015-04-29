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
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        public async Task<ActionResult> Index()
        {
            var courses = db.Courses.Include(c => c.Category).Include(c => c.OnlineProvider);
            return View(await courses.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
                ViewBag.OnlineProviderID = new SelectList(db.OnlineProviders, "ID", "Name");
                ViewBag.SessionID = new SelectList(db.Sessions, "ID", "Name");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View("Error");
            }

            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,CategoryID,Name,Credit,Code,OnlineProviderID,IsActive,SessionID")] Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Courses.Add(course);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            try
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", course.CategoryID);
                ViewBag.OnlineProviderID = new SelectList(db.OnlineProviders, "ID", "Name", course.OnlineProviderID);
                ViewBag.SessionID = new SelectList(db.Sessions, "ID", "Name");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View("Error");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            try
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", course.CategoryID);
                ViewBag.OnlineProviderID = new SelectList(db.OnlineProviders, "ID", "Name", course.OnlineProviderID);
                ViewBag.SessionID = new SelectList(db.Sessions, "ID", "Name");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View("Error");
            }

            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CategoryID,Name,Credit,Code,OnlineProviderID,IsActive,SessionID")] Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(course).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);

                }
            }


            try
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", course.CategoryID);
                ViewBag.OnlineProviderID = new SelectList(db.OnlineProviders, "ID", "Name", course.OnlineProviderID);
                ViewBag.SessionID = new SelectList(db.Sessions, "ID", "Name");
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View("Error");
            }

            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        //// POST: Courses/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Course course = await db.Courses.FindAsync(id);
        //    db.Courses.Remove(course);
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
