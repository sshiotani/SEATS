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
    [Authorize]
    public class ProviderRejectionReasonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProviderRejectionReasons
        public async Task<ActionResult> Index()
        {
            return View(await db.ProviderRejectionReasons.ToListAsync());
        }

        // GET: ProviderRejectionReasons/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderRejectionReasons providerRejectionReasons = await db.ProviderRejectionReasons.FindAsync(id);
            if (providerRejectionReasons == null)
            {
                return HttpNotFound();
            }
            return View(providerRejectionReasons);
        }

        // GET: ProviderRejectionReasons/Create
         [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProviderRejectionReasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Reason")] ProviderRejectionReasons providerRejectionReasons)
        {
            if (ModelState.IsValid)
            {
                db.ProviderRejectionReasons.Add(providerRejectionReasons);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(providerRejectionReasons);
        }

        // GET: ProviderRejectionReasons/Edit/5
         [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderRejectionReasons providerRejectionReasons = await db.ProviderRejectionReasons.FindAsync(id);
            if (providerRejectionReasons == null)
            {
                return HttpNotFound();
            }
            return View(providerRejectionReasons);
        }

        // POST: ProviderRejectionReasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Reason")] ProviderRejectionReasons providerRejectionReasons)
        {
            if (ModelState.IsValid)
            {
                db.Entry(providerRejectionReasons).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(providerRejectionReasons);
        }

        // GET: ProviderRejectionReasons/Delete/5
         [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderRejectionReasons providerRejectionReasons = await db.ProviderRejectionReasons.FindAsync(id);
            if (providerRejectionReasons == null)
            {
                return HttpNotFound();
            }
            return View(providerRejectionReasons);
        }

        // POST: ProviderRejectionReasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProviderRejectionReasons providerRejectionReasons = await db.ProviderRejectionReasons.FindAsync(id);
            db.ProviderRejectionReasons.Remove(providerRejectionReasons);
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
