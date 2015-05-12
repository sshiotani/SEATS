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
    public class PrimariesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Primaries
        public async Task<ActionResult> Index()
        {
            return View(await db.Primaries.ToListAsync());
        }

        // GET: Primaries/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Primary primary = await db.Primaries.FindAsync(id);
            if (primary == null)
            {
                return HttpNotFound();
            }
            return View(primary);
        }

        // GET: Primaries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Primaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Business_Administrator_Email,BusinessAdministratorFirstName,BusinessAdministratorLastName,BusinessAdministratorSignature,BusinessAdministratorTelephone,DateBusinessAdministratorSignature")] Primary primary)
        {
            if (ModelState.IsValid)
            {
                db.Primaries.Add(primary);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(primary);
        }

        // GET: Primaries/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Primary primary = await db.Primaries.FindAsync(id);
            if (primary == null)
            {
                return HttpNotFound();
            }
            return View(primary);
        }

        // POST: Primaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Business_Administrator_Email,BusinessAdministratorFirstName,BusinessAdministratorLastName,BusinessAdministratorSignature,BusinessAdministratorTelephone,DateBusinessAdministratorSignature")] Primary primary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(primary).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(primary);
        }

        // GET: Primaries/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Primary primary = await db.Primaries.FindAsync(id);
            if (primary == null)
            {
                return HttpNotFound();
            }
            return View(primary);
        }

        // POST: Primaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Primary primary = await db.Primaries.FindAsync(id);
            db.Primaries.Remove(primary);
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
