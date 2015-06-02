﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AutoMapper;
using CcaRegistrationDf.Models;
using System.Text.RegularExpressions;

namespace CcaRegistrationDf.Controllers
{
    [Authorize]
    public class CounselorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private SEATSEntities1 cactusDb = new SEATSEntities1();

        // GET: Counselors
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Counselors.ToListAsync());
        }

        // GET: CCAs for Counselor
        [Authorize(Roles = "Counselor")]
        public async Task<ActionResult> CcaInterface()
        {
            // Look up counselor associated with this user
            var userId = User.Identity.GetUserId();
            var counselor = await db.Counselors.Where(m => m.UserId == userId).FirstOrDefaultAsync();

            // Look up all ccas associated with this primary
            if (counselor != null)
            {
                var ccas = await db.CCAs.Where(m => m.EnrollmentLocationSchoolNamesID == counselor.EnrollmentLocationSchoolNameID).ToListAsync();

                // Create list of viewmodels populated from 
                var ccaVmList = await GetCcaViewModelList(ccas);


                // Send to form to edit these ccas
                return View(ccaVmList);
            }

            ViewBag.Message = "Unable to find Counselor associated with your user account.";
            return View("Error");

        }

        private async Task<List<CounselorCcaViewModel>> GetCcaViewModelList(List<CCA> ccas)
        {
            var ccaVmList = new List<CounselorCcaViewModel>();
            foreach (var cca in ccas)
            {
                Mapper.CreateMap<CCA, CounselorCcaViewModel>();
                var ccaVm = Mapper.Map<CCA, CounselorCcaViewModel>(cca);

                ccaVm.Session = await db.Session.Where(m => m.ID == ccaVm.SessionID).FirstOrDefaultAsync().ConfigureAwait(false);
                ccaVm.OnlineCourse = await db.Courses.Where(m => m.ID == ccaVm.OnlineCourseID).FirstOrDefaultAsync().ConfigureAwait(false);
                ccaVm.Provider = await db.Providers.Where(m => m.ID == ccaVm.ProviderID).FirstOrDefaultAsync().ConfigureAwait(false);
                ccaVm.Student = await db.Students.Where(m => m.ID == ccaVm.StudentID).FirstOrDefaultAsync().ConfigureAwait(false);
                ccaVm.CourseCredit = await db.CourseCredits.Where(m => m.ID == ccaVm.CourseCreditID).FirstOrDefaultAsync().ConfigureAwait(false);

                ccaVm.CcaID = cca.ID;

                ccaVmList.Add(ccaVm);

            }
            return ccaVmList;
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


        // GET: Counselors/Create
        public async Task<ActionResult> Create()
        {
            var leas = await cactusDb.CactusInstitutions.ToListAsync();

            leas.Insert(0, new CactusInstitution() { Code = "", Name = "District", ID = 0 });

            ViewBag.EnrollmentLocationID = new SelectList(leas, "ID", "Name");

            ViewBag.EnrollmentLocationSchoolNameID = new List<SelectListItem>();
            ViewBag.CounselorID = new List<SelectListItem>();

            return View();
        }

        // POST: Counselors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Email, FirstName, LastName, Phone, CactusID, EnrollmentLocationID, EnrollmentLocationSchoolNameID, School, CounselorID")] CounselorViewModel counselorVm)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<CounselorViewModel, Counselor>();

                Counselor counselor;

                if (counselorVm.CounselorID == 0)
                {
                    counselor = new Counselor();
                }
                else
                {
                    counselor = await db.Counselors.Where(c => c.ID == counselorVm.CounselorID).FirstOrDefaultAsync();
                }

                // Mapper causes all properties in the ViewModel to update the Counselor object so errors or not required empty properties will overwrite
                // properties in the database. (CactusID is one known problem.)
                // TODO: write own mapper type method that will check for nulls or known errors.

                if ((counselorVm.CactusID == 0 || counselorVm.CactusID == null) && counselor.CactusID != null)
                    counselorVm.CactusID = counselor.CactusID;

                counselor = Mapper.Map<CounselorViewModel, Counselor>(counselorVm, counselor);

                counselor.UserId = User.Identity.GetUserId();

                // Remove all non numeric chars
                counselor.Phone = JustDigits(counselor.Phone);


                // If counselor not found in database need to add
                if (counselor.ID == 0)
                    db.Counselors.Add(counselor);


                var count = await db.SaveChangesAsync();

                if (count != 0) // Set account setup to true if successfully added
                {
                    var user = await db.Users.Where(m => m.Id == counselor.UserId).FirstOrDefaultAsync();
                    user.IsSetup = true;
                    await db.SaveChangesAsync();
                }
                else
                {
                    ViewBag.Message = "Unable to save Counselor!";
                    return View("Error");
                }

                TempData["UserType"] = "Counselor School User";

                return RedirectToAction("EmailAdminToConfirm", "Account");
            }

            // Add model state errors since fields with errors are hidden on repost.

            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            foreach (var error in errors)
                ModelState.AddModelError("", error.Select(x => x.ErrorMessage).First());

            var leas = await cactusDb.CactusInstitutions.ToListAsync();

            leas.Insert(0, new CactusInstitution() { Code = "", Name = "District", ID = 0 });

            ViewBag.EnrollmentLocationID = new SelectList(leas, "ID", "Name");
            ViewBag.EnrollmentLocationSchoolNameID = new List<SelectListItem>();
            ViewBag.CounselorID = new List<SelectListItem>();
            return View(counselorVm);
        }


        public JsonResult GetCounselors(int schoolId)
        {
            var counselors = db.Counselors.Where(m => m.EnrollmentLocationSchoolNameID == schoolId).Select(f => new SelectListItem
            {
                Value = f.ID.ToString(),
                Text = f.FirstName + " " + f.LastName
            });

            // Add a item to add new counselor to list.

            var counselorList = counselors.AsEnumerable().Concat(new[] {new SelectListItem
                    {
                        Value = "0",
                        Text = "Counselor Not Listed."
                    }
                    });

            return Json(new SelectList(counselorList, "Value", "Text"));
        }

        public async Task<JsonResult> GetCounselorInformation(int counselorId)
        {
            var counselor = await db.Counselors.Where(c => c.ID == counselorId).FirstOrDefaultAsync().ConfigureAwait(false);

            return Json(counselor);
        }

        // GET: Counselors/Edit/5
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
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserId,CactusID,Email,FirstName,LastName,Phone,School,EnrollmentLocationID,EnrollmentLocationSchoolNameID")] Counselor counselor)
        {
            if (ModelState.IsValid)
            {
                counselor.Phone = JustDigits(counselor.Phone);

                db.Entry(counselor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(counselor);
        }

        // GET: Counselors/Delete/5
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

        /// <summary>
        /// strips out extra chars from phone number so resulting string is only numbers.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        private string JustDigits(string phoneNumber)
        {
            return Regex.Replace(phoneNumber, @"[^\d]", "");
        }
    }
}
