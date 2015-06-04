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
using AutoMapper;

namespace CcaRegistrationDf.Controllers
{
    [Authorize]
    public class ProviderUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProviderUsers
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var providerUsers = db.ProviderUsers.Include(p => p.Provider);
            return View(await providerUsers.ToListAsync().ConfigureAwait(false));
        }

        // GET: CCAs for Provider
        [Authorize(Roles = "Admin,Provider")]
        public async Task<ActionResult> CcaInterface()
        {
            // Look up all ccas associated with this provider

            // Send to form to edit these ccas
            var userId = User.Identity.GetUserId();
            var provider = await db.ProviderUsers.Where(m => m.UserId == userId).FirstOrDefaultAsync().ConfigureAwait(false);

            var ccas = await db.CCAs.Where(m => m.ProviderID == provider.ProviderID).ToListAsync().ConfigureAwait(false);

            // Create list of viewmodels populated from 
            var ccaVmList = await GetCcaViewModelList(ccas).ConfigureAwait(false);

            ViewBag.SchoolName = await db.Providers.Where(m => m.ID == provider.ProviderID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);

            // Send to form to edit these ccas
            return View(ccaVmList);
      
        }

        private async Task<List<ProviderCcaViewModel>> GetCcaViewModelList(List<CCA> ccas)
        {
            Mapper.CreateMap<CCA, ProviderCcaViewModel>();

            var ccaVmList = new List<ProviderCcaViewModel>();

            using (SEATSEntities1 cactusDb = new SEATSEntities1())
            {
                foreach (var cca in ccas)
                {
                    var ccaVm = Mapper.Map<CCA, ProviderCcaViewModel>(cca);

                    ccaVm.CcaID = cca.ID;

                    if (cca.EnrollmentLocationID < 3)
                    {
                        ccaVm.Primary = "HOME/PRIVATE SCHOOL";
                    }
                    else
                    {
                        ccaVm.Primary = await cactusDb.CactusInstitutions.Where(m => m.ID == cca.EnrollmentLocationID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);
                    }

                    ccaVmList.Add(ccaVm);

                }

                return ccaVmList;
            }
        }

        // GET: ProviderUsers/Details/5
        [Authorize(Roles = "Admin,Provider")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderUser providerUser = await db.ProviderUsers.FindAsync(id).ConfigureAwait(false);
            if (providerUser == null)
            {
                return HttpNotFound();
            }
            return View(providerUser);
        }

        // GET: ProviderUsers/Create
        public async Task<ActionResult> Create()
        {
            var providers = await db.Providers.ToListAsync().ConfigureAwait(false);

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
                var count = await db.SaveChangesAsync().ConfigureAwait(false);

                if (count != 0) // Set account setup to true if successfully added
                {
                    var user = await db.Users.Where(m => m.Id == providerUser.UserId).FirstOrDefaultAsync().ConfigureAwait(false);
                    user.IsSetup = true;
                    await db.SaveChangesAsync().ConfigureAwait(false);
                }
                else
                {
                    ViewBag.Message = "Unable to save Provider!";
                    return View("Error");
                }

                TempData["UserType"] = "Provider User";

                return RedirectToAction("EmailAdminToConfirm", "Account");
            }

            var providers = await db.Providers.ToListAsync().ConfigureAwait(false);
            providers.Insert(0, new Provider() { ID = 0, Name = "Providers", DistrictNumber = "0" });
            ViewBag.ProviderID = new SelectList(providers, "ID", "Name");
            
            return View(providerUser);
        }

        // GET: ProviderUsers/Edit/5
        [Authorize(Roles = "Admin,Provider")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderUser providerUser = await db.ProviderUsers.FindAsync(id).ConfigureAwait(false);
            if (providerUser == null)
            {
                return HttpNotFound();
            }

            var providers = await db.Providers.ToListAsync().ConfigureAwait(false);
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
                await db.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction("Index");
            }
            var providers = await db.Providers.ToListAsync().ConfigureAwait(false);
            providers.Insert(0, new Provider() { ID = 0, Name = "Providers", DistrictNumber = "0" });
            ViewBag.ProviderID = new SelectList(providers, "ID", "Name");

            return View(providerUser);
        }

        // GET: ProviderUsers/Delete/5
       
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderUser providerUser = await db.ProviderUsers.FindAsync(id).ConfigureAwait(false);
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
            ProviderUser providerUser = await db.ProviderUsers.FindAsync(id).ConfigureAwait(false);
            db.ProviderUsers.Remove(providerUser);
            var user = await db.Users.Where(m => m.Id == providerUser.UserId).FirstOrDefaultAsync().ConfigureAwait(false);
            user.IsSetup = false;
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
