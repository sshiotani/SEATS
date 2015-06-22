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
    public class ExcessiveFEDReasonsController : Controller
    {
        private ApplicationDbContext db;

        //private SeatsContext db { get; set; }

        public ExcessiveFEDReasonsController()
        {
            this.db = new ApplicationDbContext();
        }

        // GET: ExcessiveFEDReasons
        public async Task<ActionResult> Index()
        {
            return View(await db.ExcessiveFEDReasons.ToListAsync());
        }

        // GET: ExcessiveFEDReasons/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExcessiveFEDReason excessiveFEDReason = await db.ExcessiveFEDReasons.FindAsync(id);
            if (excessiveFEDReason == null)
            {
                return HttpNotFound();
            }
            return View(excessiveFEDReason);
        }

        // GET: ExcessiveFEDReasons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExcessiveFEDReasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Reason")] ExcessiveFEDReason excessiveFEDReason)
        {
            if (ModelState.IsValid)
            {
                db.ExcessiveFEDReasons.Add(excessiveFEDReason);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(excessiveFEDReason);
        }

        // GET: ExcessiveFEDReasons/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExcessiveFEDReason excessiveFEDReason = await db.ExcessiveFEDReasons.FindAsync(id);
            if (excessiveFEDReason == null)
            {
                return HttpNotFound();
            }
            return View(excessiveFEDReason);
        }

        // POST: ExcessiveFEDReasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Reason")] ExcessiveFEDReason excessiveFEDReason)
        {
            if (ModelState.IsValid)
            {
                db.Entry(excessiveFEDReason).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(excessiveFEDReason);
        }

        // GET: ExcessiveFEDReasons/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExcessiveFEDReason excessiveFEDReason = await db.ExcessiveFEDReasons.FindAsync(id);
            if (excessiveFEDReason == null)
            {
                return HttpNotFound();
            }
            return View(excessiveFEDReason);
        }

        // POST: ExcessiveFEDReasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ExcessiveFEDReason excessiveFEDReason = await db.ExcessiveFEDReasons.FindAsync(id);
            db.ExcessiveFEDReasons.Remove(excessiveFEDReason);
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
