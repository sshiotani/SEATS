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
    public class PrimaryUsersController : Controller
    {
        private ApplicationDbContext db;
        private SEATSEntities cactus = new SEATSEntities();
        
        //private SeatsContext db { get; set; }

        public PrimaryUsersController()
        {
            this.db = new ApplicationDbContext();
        }

        // GET: PrimaryUsers
        [Authorize(Roles="Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.PrimaryUsers.ToListAsync().ConfigureAwait(false));
        }

        // GET: CCAs for primary
        [Authorize(Roles = "Primary")]
        public async Task<ActionResult> CcaInterface()
        {
            // Look up all ccas associated with this primary
            var userId = User.Identity.GetUserId();
            var school = await db.PrimaryUsers.Where(m => m.UserId == userId).FirstOrDefaultAsync().ConfigureAwait(false);

            var ccas = await db.CCAs.Where(m => m.EnrollmentLocationSchoolNamesID == school.EnrollmentLocationSchoolNameID).ToListAsync().ConfigureAwait(false);

            // Create list of viewmodels populated from 
            var ccaVmList = GetCcaViewModelList(ccas);

            ViewBag.SchoolName = await cactus.CactusSchools.Where(m => m.ID == school.EnrollmentLocationSchoolNameID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);

            // Send to form to edit these ccas
            return View(ccaVmList);
        }

        private List<PrimaryCcaViewModel> GetCcaViewModelList(List<CCA> ccas)
        {
            Mapper.CreateMap<CCA, PrimaryCcaViewModel>();

            var ccaVmList = new List<PrimaryCcaViewModel>();
            foreach (var cca in ccas)
            {
                var ccaVm = Mapper.Map<CCA, PrimaryCcaViewModel>(cca);

                ccaVm.CcaID = cca.ID;

                ccaVmList.Add(ccaVm);

            }
            return ccaVmList;
        }
        // GET: PrimaryUsers/Details/5
         [Authorize(Roles = "Primary")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrimaryUser primaryUser = await db.PrimaryUsers.FindAsync(id).ConfigureAwait(false);
            if (primaryUser == null)
            {
                return HttpNotFound();
            }
            return View(primaryUser);
        }

        // GET: PrimaryUsers/Create
        public async  Task<ActionResult> Create()
        {
            var leas = await cactus.CactusInstitutions.ToListAsync().ConfigureAwait(false);

            leas.Insert(0, new CactusInstitution() { Code = "", Name = "District", ID = 0 });

            ViewBag.EnrollmentLocationID = new SelectList(leas, "ID", "Name");
            ViewBag.EnrollmentLocationSchoolNameID = new List<SelectListItem>();
            return View();
        }

        // POST: PrimaryUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Email,FirstName,LastName,Phone,EnrollmentLocationID,EnrollmentLocationSchoolNameID")] PrimaryUser primaryUser)
        {
            if (ModelState.IsValid)
            {
                primaryUser.UserId = User.Identity.GetUserId();
                var identityUser = db.Users.Find(primaryUser.UserId);
                primaryUser.Email = identityUser.Email;

                db.PrimaryUsers.Add(primaryUser);
                var count = await db.SaveChangesAsync();

                if (count != 0) // Set account setup to true if successfully added
                {
                    var user = db.Users.Find(primaryUser.UserId);
                    
                    user.IsSetup = true;
                    await db.SaveChangesAsync().ConfigureAwait(false);
                }
                else
                {
                    ViewBag.Message = "Unable to save Primary User!";
                    return View("Error");
                }

                TempData["UserType"] = "Primary School User";

                return RedirectToAction("EmailAdminToConfirm", "Account");
            }

            var leas = await cactus.CactusInstitutions.ToListAsync().ConfigureAwait(false);

            leas.Insert(0, new CactusInstitution() { Code = "", Name = "District", ID = 0});

            ViewBag.EnrollmentLocationID = new SelectList(leas, "ID", "Name");
            ViewBag.EnrollmentLocationSchoolNameID = new List<SelectListItem>();

            return View(primaryUser);
        }

        // GET: PrimaryUsers/Edit/5
         [Authorize(Roles = "Primary")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrimaryUser primaryUser = await db.PrimaryUsers.FindAsync(id).ConfigureAwait(false);
            if (primaryUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.EnrollmentLocationID = new SelectList(cactus.CactusInstitutions, "ID", "Name");
            ViewBag.EnrollmentLocationSchoolNameID = new List<SelectListItem>();
            return View(primaryUser);
        }

        // POST: PrimaryUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserId,Email,FirstName,LastName,IsSigned,Phone,EnrollmentLocationID,EnrollmentLocationSchoolNameID")] PrimaryUser primaryUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(primaryUser).State = EntityState.Modified;
                await db.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction("Index");
            }

            ViewBag.EnrollmentLocationID = new SelectList(cactus.CactusInstitutions, "ID", "Name");
            ViewBag.EnrollmentLocationSchoolNameID = new List<SelectListItem>();
            return View(primaryUser);
        }

        // GET: PrimaryUsers/Delete/5
        [Authorize(Roles="Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrimaryUser primaryUser = await db.PrimaryUsers.FindAsync(id).ConfigureAwait(false);
            if (primaryUser == null)
            {
                return HttpNotFound();
            }
            return View(primaryUser);
        }

        // POST: PrimaryUsers/Delete/5
         [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PrimaryUser primaryUser = await db.PrimaryUsers.FindAsync(id).ConfigureAwait(false);
            db.PrimaryUsers.Remove(primaryUser);
            var user = db.Users.Find(primaryUser.UserId);
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
