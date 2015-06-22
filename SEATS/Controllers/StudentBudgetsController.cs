using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEATS.Models;

namespace SEATS.Controllers
{
    [Authorize(Roles="Admin")]
    public class StudentBudgetsController : Controller
    {
        private ApplicationDbContext db;
        //private SeatsContext db { get; set; }
       
        public StudentBudgetsController()
        {
            this.db = new ApplicationDbContext();
           
        }

        // GET: StudentBudgets
        public async Task<ActionResult> Index()
        {
            return View(await db.StudentBudgets.ToListAsync());
        }

        // GET: StudentBudgets/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentBudget studentBudget = await db.StudentBudgets.FindAsync(id);
            if (studentBudget == null)
            {
                return HttpNotFound();
            }
            return View(studentBudget);
        }

        // GET: StudentBudgets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentBudgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Year,Month,OffSet,Distribution")] StudentBudget studentBudget)
        {
            if (ModelState.IsValid)
            {
                db.StudentBudgets.Add(studentBudget);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(studentBudget);
        }

        // GET: StudentBudgets/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentBudget studentBudget = await db.StudentBudgets.FindAsync(id);
            if (studentBudget == null)
            {
                return HttpNotFound();
            }
            return View(studentBudget);
        }

        // POST: StudentBudgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Year,Month,OffSet,Distribution")] StudentBudget studentBudget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentBudget).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(studentBudget);
        }

        // GET: StudentBudgets/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentBudget studentBudget = await db.StudentBudgets.FindAsync(id);
            if (studentBudget == null)
            {
                return HttpNotFound();
            }
            return View(studentBudget);
        }

        // POST: StudentBudgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            StudentBudget studentBudget = await db.StudentBudgets.FindAsync(id);
            db.StudentBudgets.Remove(studentBudget);
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
