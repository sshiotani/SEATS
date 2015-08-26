using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEATS.Models;

namespace SEATS.Controllers
{
    public class CounselorRejectionReasonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CounselorRejectionReasons
        public async Task<ActionResult> Index()
        {
            return View(await db.CounselorRejectionReasons.ToListAsync());
        }

        // GET: CounselorRejectionReasons/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CounselorRejectionReasons counselorRejectionReasons = await db.CounselorRejectionReasons.FindAsync(id);
            if (counselorRejectionReasons == null)
            {
                return HttpNotFound();
            }
            return View(counselorRejectionReasons);
        }

        // GET: CounselorRejectionReasons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CounselorRejectionReasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Reason")] CounselorRejectionReasons counselorRejectionReasons)
        {
            if (ModelState.IsValid)
            {
                db.CounselorRejectionReasons.Add(counselorRejectionReasons);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(counselorRejectionReasons);
        }

        // GET: CounselorRejectionReasons/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CounselorRejectionReasons counselorRejectionReasons = await db.CounselorRejectionReasons.FindAsync(id);
            if (counselorRejectionReasons == null)
            {
                return HttpNotFound();
            }
            return View(counselorRejectionReasons);
        }

        // POST: CounselorRejectionReasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Reason")] CounselorRejectionReasons counselorRejectionReasons)
        {
            if (ModelState.IsValid)
            {
                db.Entry(counselorRejectionReasons).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(counselorRejectionReasons);
        }

        // GET: CounselorRejectionReasons/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CounselorRejectionReasons counselorRejectionReasons = await db.CounselorRejectionReasons.FindAsync(id);
            if (counselorRejectionReasons == null)
            {
                return HttpNotFound();
            }
            return View(counselorRejectionReasons);
        }

        // POST: CounselorRejectionReasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CounselorRejectionReasons counselorRejectionReasons = await db.CounselorRejectionReasons.FindAsync(id);
            db.CounselorRejectionReasons.Remove(counselorRejectionReasons);
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
