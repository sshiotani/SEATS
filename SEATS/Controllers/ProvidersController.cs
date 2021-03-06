﻿using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SEATS.Models;

namespace SEATS.Controllers
{
    [Authorize]
    public class ProvidersController : Controller
    {
        private ApplicationDbContext db;

        //private SeatsContext db { get; set; }

        public ProvidersController()
        {
            this.db = new ApplicationDbContext();
        }

        // GET: Providers
        [Authorize(Roles="Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Providers.ToListAsync().ConfigureAwait(false));
        }

        // GET: Providers/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = await db.Providers.FindAsync(id).ConfigureAwait(false);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        // GET: Providers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Providers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,DistrictNumber,IsActive,Email")] Provider provider)
        {
            if (ModelState.IsValid)
            {
                //var name = User.Identity.Name;
                //provider.UserId = await db.Users.Where(m => m.UserName == name).Select(m => m.Id).FirstOrDefaultAsync();
                db.Providers.Add(provider);
                var count = await db.SaveChangesAsync().ConfigureAwait(false);

               

                return RedirectToAction("Index");
            }

            return View(provider);
        }

        // GET: Providers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = await db.Providers.FindAsync(id).ConfigureAwait(false);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        // POST: Providers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,DistrictNumber,IsActive,Email")] Provider provider)
        {
            if (ModelState.IsValid)
            {
                db.Entry(provider).State = EntityState.Modified;
                await db.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction("Index");
            }
            return View(provider);
        }

        // GET: Providers/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Message = "Delete Providers disabled! Disable provider if no longer valid.";
            return View("Error");
        }

        //[Authorize(Roles = "Admin")]
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    ViewBag.Message = "Delete Providers disabled! Disable provider if no longer valid.";
        //    return View("Error");

        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Provider provider = await db.Providers.FindAsync(id);
        //    if (provider == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(provider);
        //}

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Provider provider = await db.Providers.FindAsync(id).ConfigureAwait(false);
            db.Providers.Remove(provider);
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
