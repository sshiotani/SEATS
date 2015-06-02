using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CcaRegistrationDf.Models;

namespace CcaRegistrationDf.Controllers
{
    [Authorize(Roles="Admin,Provider")]
    public class ProviderUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProviderUsers
        public async Task<ActionResult> Index()
        {
            var providerUsers = db.ProviderUsers.Include(p => p.Provider);
            return View(await providerUsers.ToListAsync());
        }

        // GET: CCAs for Provider
        public async Task<ActionResult> CcaInterface()
        {
            // Look up all ccas associated with this primary
            // Send to form to edit these ccas
            return View();
        }

        // GET: ProviderUsers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderUser providerUser = await db.ProviderUsers.FindAsync(id);
            if (providerUser == null)
            {
                return HttpNotFound();
            }
            return View(providerUser);
        }

        // GET: ProviderUsers/Create
        public async Task<ActionResult> Create()
        {
            var providers = await db.Providers.ToListAsync();

            providers.Insert(0, new Provider() { ID = 0, Name = "Providers", DistrictNumber = "0" });

            ViewBag.ProviderID = new SelectList(providers, "ID", "Name");
            return View();
        }

        // POST: ProviderUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FirstName,LastName,Phone,Fax,ProviderID")] ProviderUser providerUser)
        {
            if (ModelState.IsValid)
            {
                providerUser.UserId = User.Identity.GetUserId();
                db.ProviderUsers.Add(providerUser);
                var count = await db.SaveChangesAsync();

                if (count != 0) // Set account setup to true if successfully added
                {
                    var user = await db.Users.Where(m => m.Id == providerUser.UserId).FirstOrDefaultAsync();
                    user.IsSetup = true;
                    await db.SaveChangesAsync();
                }
                else
                {
                    ViewBag.Message = "Unable to save Provider!";
                    return View("Error");
                }

                TempData["UserType"] = "Provider User";

                return RedirectToAction("EmailAdminToConfirm", "Account");
            }

            var providers = await db.Providers.ToListAsync();
            providers.Insert(0, new Provider() { ID = 0, Name = "Providers", DistrictNumber = "0" });
            ViewBag.ProviderID = new SelectList(providers, "ID", "Name");
            
            return View(providerUser);
        }

        // GET: ProviderUsers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderUser providerUser = await db.ProviderUsers.FindAsync(id);
            if (providerUser == null)
            {
                return HttpNotFound();
            }

            var providers = await db.Providers.ToListAsync();
            providers.Insert(0, new Provider() { ID = 0, Name = "Providers", DistrictNumber = "0" });
            ViewBag.ProviderID = new SelectList(providers, "ID", "Name");
            
            return View(providerUser);
        }

        // POST: ProviderUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserId,FirstName,LastName,Phone,Email,Fax,ProviderID")] ProviderUser providerUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(providerUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var providers = await db.Providers.ToListAsync();
            providers.Insert(0, new Provider() { ID = 0, Name = "Providers", DistrictNumber = "0" });
            ViewBag.ProviderID = new SelectList(providers, "ID", "Name");

            return View(providerUser);
        }

        // GET: ProviderUsers/Delete/5
        [Authorize(Roles="Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderUser providerUser = await db.ProviderUsers.FindAsync(id);
            if (providerUser == null)
            {
                return HttpNotFound();
            }
            return View(providerUser);
        }

        // POST: ProviderUsers/Delete/5
         [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProviderUser providerUser = await db.ProviderUsers.FindAsync(id);
            db.ProviderUsers.Remove(providerUser);
            var user = await db.Users.Where(m => m.Id == providerUser.UserId).FirstOrDefaultAsync();
            user.IsSetup = false;
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
