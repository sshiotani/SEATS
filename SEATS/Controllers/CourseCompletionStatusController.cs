using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SEATS.Models;

namespace SEATS.Controllers
{
    public class CourseCompletionStatusController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CourseCompletionStatus
        public async Task<ActionResult> Index()
        {
            return View(await db.CourseCompletionStatus.ToListAsync());
        }

        // GET: CourseCompletionStatus/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCompletionStatus courseCompletionStatus = await db.CourseCompletionStatus.FindAsync(id);
            if (courseCompletionStatus == null)
            {
                return HttpNotFound();
            }
            return View(courseCompletionStatus);
        }

        // GET: CourseCompletionStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseCompletionStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Status")] CourseCompletionStatus courseCompletionStatus)
        {
            if (ModelState.IsValid)
            {
                db.CourseCompletionStatus.Add(courseCompletionStatus);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(courseCompletionStatus);
        }

        // GET: CourseCompletionStatus/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCompletionStatus courseCompletionStatus = await db.CourseCompletionStatus.FindAsync(id);
            if (courseCompletionStatus == null)
            {
                return HttpNotFound();
            }
            return View(courseCompletionStatus);
        }

        // POST: CourseCompletionStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Status")] CourseCompletionStatus courseCompletionStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseCompletionStatus).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(courseCompletionStatus);
        }

        // GET: CourseCompletionStatus/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCompletionStatus courseCompletionStatus = await db.CourseCompletionStatus.FindAsync(id);
            if (courseCompletionStatus == null)
            {
                return HttpNotFound();
            }
            return View(courseCompletionStatus);
        }

        // POST: CourseCompletionStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CourseCompletionStatus courseCompletionStatus = await db.CourseCompletionStatus.FindAsync(id);
            db.CourseCompletionStatus.Remove(courseCompletionStatus);
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
