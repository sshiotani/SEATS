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
    public class PrimaryRejectionReasonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PrimaryRejectionReasons
        public async Task<ActionResult> Index()
        {
            return View(await db.PrimaryRejectionReasons.ToListAsync());
        }

        // GET: PrimaryRejectionReasons/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrimaryRejectionReasons primaryRejectionReasons = await db.PrimaryRejectionReasons.FindAsync(id);
            if (primaryRejectionReasons == null)
            {
                return HttpNotFound();
            }
            return View(primaryRejectionReasons);
        }

        // GET: PrimaryRejectionReasons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PrimaryRejectionReasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Reason,Entity")] PrimaryRejectionReasons primaryRejectionReasons)
        {
            if (ModelState.IsValid)
            {
                db.PrimaryRejectionReasons.Add(primaryRejectionReasons);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(primaryRejectionReasons);
        }

        // GET: PrimaryRejectionReasons/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrimaryRejectionReasons primaryRejectionReasons = await db.PrimaryRejectionReasons.FindAsync(id);
            if (primaryRejectionReasons == null)
            {
                return HttpNotFound();
            }
            return View(primaryRejectionReasons);
        }

        // POST: PrimaryRejectionReasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Reason,Entity")] PrimaryRejectionReasons primaryRejectionReasons)
        {
            if (ModelState.IsValid)
            {
                db.Entry(primaryRejectionReasons).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(primaryRejectionReasons);
        }

        // GET: PrimaryRejectionReasons/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrimaryRejectionReasons primaryRejectionReasons = await db.PrimaryRejectionReasons.FindAsync(id);
            if (primaryRejectionReasons == null)
            {
                return HttpNotFound();
            }
            return View(primaryRejectionReasons);
        }

        // POST: PrimaryRejectionReasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PrimaryRejectionReasons primaryRejectionReasons = await db.PrimaryRejectionReasons.FindAsync(id);
            db.PrimaryRejectionReasons.Remove(primaryRejectionReasons);
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
