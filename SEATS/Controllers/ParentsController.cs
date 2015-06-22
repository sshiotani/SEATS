using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;

using SEATS.Models;
using Microsoft.AspNet.Identity;

namespace SEATS.Controllers
{
    [Authorize]
    public class ParentsController : Controller
    {
        private ApplicationDbContext db;

        //private SeatsContext db { get; set; }

        public ParentsController()
        {
            this.db = new ApplicationDbContext();
        }

        // GET: Parents
        [Authorize(Roles = ("Admin"))]
        public async Task<ActionResult> Index()
        {
            var parents = await db.Parents.ToListAsync().ConfigureAwait(false);
            return View(parents);

        }

        // GET: Parents/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = await db.Parents.FindAsync(id).ConfigureAwait(false);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // GET: Parents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Parents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,GuardianEmail,GuardianFirstName,GuardianLastName,GuardianPhone1,GuardianPhone2")] Parent parent)
        {
            if (ModelState.IsValid)
            {
                // Check email to see if parent already parentID

                int parentID = await db.Parents.Where(m => m.GuardianEmail == parent.GuardianEmail).Select(m => m.ID).FirstOrDefaultAsync().ConfigureAwait(false);

                //If not add them
                if (parentID == 0 || parentID == null)
                {
                    db.Parents.Add(parent);
                    await db.SaveChangesAsync().ConfigureAwait(false);
                    parentID = await db.Parents.Where(m => m.GuardianEmail == parent.GuardianEmail).Select(m => m.ID).FirstOrDefaultAsync().ConfigureAwait(false);
                }

                // Add parent Id to student

                var UserIdentity = User.Identity.GetUserId();
                var student = await db.Students.Where(u => u.UserId == UserIdentity).FirstOrDefaultAsync().ConfigureAwait(false);

                student.ParentID = (int?)parentID;

                if (student.ParentID == null || parentID == 0)
                {
                    ViewBag.Message = "Unable to associate parent information with student.  Please contact help desk.";
                    return View("Error");
                }

                // Account registration is complete mark account as setup.

                var user = db.Users.Find(student.UserId);
                user.IsSetup = true;

                await db.SaveChangesAsync().ConfigureAwait(false);

                return RedirectToAction("Index", "Home");
            }

            return View(parent);
        }

        // GET: Parents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = await db.Parents.FindAsync(id).ConfigureAwait(false);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // POST: Parents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,GuardianEmail,GuardianFirstName,GuardianLastName,GuardianPhone1,GuardianPhone2")] Parent parent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parent).State = EntityState.Modified;
                await db.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToAction("Index");
            }
            return View(parent);
        }

        // GET: Parents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = await db.Parents.FindAsync(id).ConfigureAwait(false);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // POST: Parents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Parent parent = await db.Parents.FindAsync(id).ConfigureAwait(false);
            db.Parents.Remove(parent);
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
