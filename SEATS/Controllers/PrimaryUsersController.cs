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
using SEATS.Models;
using AutoMapper;

namespace SEATS.Controllers
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
            var userList = new List<PrimaryUserViewModel>();
            var primaryUsers = await db.PrimaryUsers.ToListAsync();

            foreach (var user in primaryUsers)
            {
                var userVm = new PrimaryUserViewModel();
                userVm.PrimaryUser = user;
                userVm.Lea = await cactus.CactusInstitutions.Where(m => m.ID == user.EnrollmentLocationID).Select(c => c.Name).FirstOrDefaultAsync();
                //userVm.School = await cactus.CactusSchools.Where(m => m.ID == user.EnrollmentLocationSchoolNameID).Select(c => c.Name).FirstAsync();
                userList.Add(userVm);
            }


           return View(userList);
        }

        // GET: CCAs for primary
        [Authorize(Roles = "Primary,Admin")]
        public async Task<ActionResult> CcaInterface()
        {
            // Look up all ccas associated with this primary
            var userId = User.Identity.GetUserId();
            var lea = await db.PrimaryUsers.FirstOrDefaultAsync(m => m.UserId == userId).ConfigureAwait(false);

            var ccas = await db.CCAs.Where(m => m.EnrollmentLocationID == lea.EnrollmentLocationID).ToListAsync().ConfigureAwait(false);

            // Create list of viewmodels populated from 
            var ccaVmList = GetCcaViewModelList(ccas);

            ViewBag.LeaName = await cactus.CactusInstitutions.Where(m => m.ID == lea.EnrollmentLocationID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);

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
         [Authorize(Roles = "Primary,Admin")]
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
           // ViewBag.EnrollmentLocationSchoolNameID = new List<SelectListItem>();
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
                //primaryUser.Email = identityUser.Email;

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

            ViewBag.EnrollmentLocationID = new SelectList(leas, "ID", "Name",primaryUser.EnrollmentLocationID);
            ViewBag.EnrollmentLocationSchoolNameID = new List<SelectListItem>();

            return View(primaryUser);
        }

        // GET: PrimaryUsers/Edit/5
         [Authorize(Roles = "Primary,Admin")]
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

            ViewBag.EnrollmentLocationID = new SelectList(cactus.CactusInstitutions, "ID", "Name", primaryUser.EnrollmentLocationID);
            ViewBag.EnrollmentLocationSchoolNameID = new SelectList(cactus.CactusSchools, "ID", "Name", primaryUser.EnrollmentLocationSchoolNameID);
            return View(primaryUser);
        }

        // POST: PrimaryUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserId,Email,FirstName,LastName,Phone,EnrollmentLocationID,EnrollmentLocationSchoolNameID")] PrimaryUser primaryUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(primaryUser).State = EntityState.Modified;
                await db.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction("Index");
            }

            ViewBag.EnrollmentLocationID = new SelectList(cactus.CactusInstitutions, "ID", "Name",primaryUser.EnrollmentLocationID);
            ViewBag.EnrollmentLocationSchoolNameID = new SelectList(cactus.CactusSchools, "ID", "Name", primaryUser.EnrollmentLocationSchoolNameID);
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
