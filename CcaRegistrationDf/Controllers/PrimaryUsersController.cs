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
using CcaRegistrationDf.Models.StudentCcas;

namespace CcaRegistrationDf.Controllers
{
    [Authorize]
    public class PrimaryUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CactusEntities cactusDb = new CactusEntities();

        // GET: PrimaryUsers
        public async Task<ActionResult> Index()
        {
            return View(await db.PrimaryUsers.ToListAsync());
        }

        // GET: PrimaryUsers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrimaryUser primaryUser = await db.PrimaryUsers.FindAsync(id);
            if (primaryUser == null)
            {
                return HttpNotFound();
            }
            return View(primaryUser);
        }

        // GET: PrimaryUsers/Create
        public ActionResult Create()
        {
            ViewBag.EnrollmentLocationID = new SelectList(cactusDb.CactusInstitutions, "DistrictID", "Name");
            ViewBag.EnrollmentLocationSchoolNameID = new List<SelectListItem>();
            return View();
        }

        // POST: PrimaryUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Email,FirstName,LastName,Phone,EnrollmentLocationID")] PrimaryUser primaryUser)
        {
            if (ModelState.IsValid)
            {
                primaryUser.UserId = User.Identity.GetUserId();
                db.PrimaryUsers.Add(primaryUser);
                var count = await db.SaveChangesAsync();

                if (count != 0) // Set account setup to true if successfully added
                {
                    var user = await db.Users.Where(m => m.Id == primaryUser.UserId).FirstOrDefaultAsync();
                    user.IsSetup = true;
                    await db.SaveChangesAsync();
                }
                else
                {
                    ViewBag.Message = "Unable to save Primary User!";
                    return View("Error");
                }

                TempData["UserType"] = "Primary School User";

                return RedirectToAction("EmailAdminToConfirm", "Account");
            }

            return View(primaryUser);
        }

        // GET: PrimaryUsers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrimaryUser primaryUser = await db.PrimaryUsers.FindAsync(id);
            if (primaryUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.EnrollmentLocationID = new SelectList(cactusDb.CactusInstitutions, "DistrictID", "Name");
            ViewBag.EnrollmentLocationSchoolNameID = new List<SelectListItem>();
            return View(primaryUser);
        }

        // POST: PrimaryUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserId,Email,FirstName,LastName,IsSigned,Phone,EnrollmentLocationID")] PrimaryUser primaryUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(primaryUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EnrollmentLocationID = new SelectList(cactusDb.CactusInstitutions, "DistrictID", "Name");
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
            PrimaryUser primaryUser = await db.PrimaryUsers.FindAsync(id);
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
            PrimaryUser primaryUser = await db.PrimaryUsers.FindAsync(id);
            db.PrimaryUsers.Remove(primaryUser);
            var user = await db.Users.Where(m => m.Id == primaryUser.UserId).FirstOrDefaultAsync();
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
