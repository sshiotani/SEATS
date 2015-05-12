using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CcaRegistrationDf.DAL;
using CcaRegistrationDf.Models;

namespace CcaRegistrationDf.Controllers
{
    [Authorize(Roles="Admin")]
    public class CourseCreditsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CourseCredits
        public async Task<ActionResult> Index()
        {
            return View(await db.CourseCredits.ToListAsync());
        }

        // GET: CourseCredits/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCredit courseCredit = await db.CourseCredits.FindAsync(id);
            if (courseCredit == null)
            {
                return HttpNotFound();
            }
            return View(courseCredit);
        }

        // GET: CourseCredits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseCredits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Value")] CourseCredit courseCredit)
        {
            if (ModelState.IsValid)
            {
                db.CourseCredits.Add(courseCredit);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(courseCredit);
        }

        // GET: CourseCredits/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCredit courseCredit = await db.CourseCredits.FindAsync(id);
            if (courseCredit == null)
            {
                return HttpNotFound();
            }
            return View(courseCredit);
        }

        // POST: CourseCredits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Value")] CourseCredit courseCredit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseCredit).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(courseCredit);
        }

        // GET: CourseCredits/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCredit courseCredit = await db.CourseCredits.FindAsync(id);
            if (courseCredit == null)
            {
                return HttpNotFound();
            }
            return View(courseCredit);
        }

        // POST: CourseCredits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CourseCredit courseCredit = await db.CourseCredits.FindAsync(id);
            db.CourseCredits.Remove(courseCredit);
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
