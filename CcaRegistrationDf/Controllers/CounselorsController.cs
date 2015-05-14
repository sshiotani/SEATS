﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class CounselorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Counselors
        public async Task<ActionResult> Index()
        {
            return View(await db.Counselors.ToListAsync());
        }

        // GET: Counselors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Counselor counselor = await db.Counselors.FindAsync(id);
            if (counselor == null)
            {
                return HttpNotFound();
            }
            return View(counselor);
        }

        // GET: Counselors/Details/5
        public async Task<ActionResult> UpdateSchoolIds()
        {
            var toUpdate = await db.Counselors.Where(x => x.SchoolID == 0).ToListAsync();

            using (CactusEntities cactus = new CactusEntities())
            {
                foreach (var counselor in toUpdate)
                {
                    var schoolId = cactus.CactusSchools.Where(x => x.name == counselor.School).Select(x => x.id).FirstOrDefault();

                    counselor.SchoolID = Decimal.ToInt32(schoolId);
                }
            }

          

            await db.SaveChangesAsync();

            return View();
        }

        // GET: Counselors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Counselors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,CounselorCactusID,CounselorEmail,CounselorFirstName,CounselorLastName,CounselorPhoneNumber,SchoolID")] CounselorViewModel counselorVm)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<CounselorViewModel, Counselor>();
                Counselor counselor = Mapper.Map<CounselorViewModel, Counselor>(counselorVm);
                db.Counselors.Add(counselor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(counselorVm);
        }

        // GET: Counselors/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Counselor counselor = await db.Counselors.FindAsync(id);
            if (counselor == null)
            {
                return HttpNotFound();
            }
            return View(counselor);
        }

        // POST: Counselors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CounselorCactusID,CounselorEmail,CounselorFirstName,CounselorLastName,CounselorPhoneNumber,SchoolID")] Counselor counselor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(counselor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(counselor);
        }

        // GET: Counselors/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Counselor counselor = await db.Counselors.FindAsync(id);
            if (counselor == null)
            {
                return HttpNotFound();
            }
            return View(counselor);
        }

        // POST: Counselors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Counselor counselor = await db.Counselors.FindAsync(id);
            db.Counselors.Remove(counselor);
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
