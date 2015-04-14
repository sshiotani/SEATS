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
    public class OnlineProvidersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OnlineProviders
        public async Task<ActionResult> Index()
        {
            return View(await db.OnlineProviders.ToListAsync());
        }

        // GET: OnlineProviders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineProvider onlineProvider = await db.OnlineProviders.FindAsync(id);
            if (onlineProvider == null)
            {
                return HttpNotFound();
            }
            return View(onlineProvider);
        }

        // GET: OnlineProviders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OnlineProviders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,DistrictID")] OnlineProvider onlineProvider)
        {
            if (ModelState.IsValid)
            {
                db.OnlineProviders.Add(onlineProvider);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(onlineProvider);
        }

        // GET: OnlineProviders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineProvider onlineProvider = await db.OnlineProviders.FindAsync(id);
            if (onlineProvider == null)
            {
                return HttpNotFound();
            }
            return View(onlineProvider);
        }

        // POST: OnlineProviders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,DistrictID")] OnlineProvider onlineProvider)
        {
            if (ModelState.IsValid)
            {
                db.Entry(onlineProvider).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(onlineProvider);
        }

        // GET: OnlineProviders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineProvider onlineProvider = await db.OnlineProviders.FindAsync(id);
            if (onlineProvider == null)
            {
                return HttpNotFound();
            }
            return View(onlineProvider);
        }

        // POST: OnlineProviders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OnlineProvider onlineProvider = await db.OnlineProviders.FindAsync(id);
            db.OnlineProviders.Remove(onlineProvider);
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
