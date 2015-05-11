using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using CcaRegistrationDf.DAL;
using CcaRegistrationDf.Models;
using Microsoft.AspNet.Identity;

namespace CcaRegistrationDf.Controllers
{
    [Authorize]
    public class ParentsController : Controller
    {
        private SoepContext db = new SoepContext();

        // GET: Parents
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var parents = await db.Parents.ToListAsync();
                return View(parents);
            }

            // find Student info associated with user and check for existing parent association

            var UserIdentity = User.Identity.GetUserId();

            var student = await db.Students.Where(u => u.UserId == UserIdentity).FirstOrDefaultAsync();

            if(student.ParentID != null)
            {
                var parent = await db.Parents.Where(u => u.ID == student.ParentID).FirstOrDefaultAsync();
                if (parent != null)
                {
                    return View(parent);
                }
                
            }

            //Otherwise create one

            return RedirectToAction("Create");
        }

        // GET: Parents/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = await db.Parents.FindAsync(id);
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

                int? parentID = await db.Parents.Where(m => m.GuardianEmail == parent.GuardianEmail).Select(m=>m.ID).FirstOrDefaultAsync();

               //If not add them
                if(parentID == null)
                {
                    db.Parents.Add(parent);
                    parentID = await db.Parents.Where(m => m.GuardianEmail == parent.GuardianEmail).Select(m => m.ID).FirstOrDefaultAsync();
                }

                // Add parent Id to student
                var UserIdentity = User.Identity.GetUserId();
                var student = await db.Students.Where(u => u.UserId == UserIdentity).FirstOrDefaultAsync();

                // Find parent with
                student.ParentID = parentID;

                if(student.ParentID == null)
                {
                    ViewBag.Message = "Unable to associate parent information with student.  Please contact help desk.";
                    return View("Error");
                }

                await db.SaveChangesAsync();

                return RedirectToAction("Index","Home");
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
            Parent parent = await db.Parents.FindAsync(id);
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
                await db.SaveChangesAsync();
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
            Parent parent = await db.Parents.FindAsync(id);
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
            Parent parent = await db.Parents.FindAsync(id);
            db.Parents.Remove(parent);
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
