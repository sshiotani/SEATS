using System;
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
using SEATS.Models;
using System.Text.RegularExpressions;

namespace SEATS.Controllers
{
    [Authorize]
    public class CounselorsController : Controller
    {
        private ApplicationDbContext db;
        //private SeatsContext db { get; set; }
        private SEATSEntities cactus;

       
        public CounselorsController()
        {
            this.db = new ApplicationDbContext();
           // this.db = db;
            this.cactus  = new SEATSEntities();
        }

        // GET: Counselors
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Counselors.ToListAsync().ConfigureAwait(false));
        }

        // GET: CCAs for Counselor
        [Authorize(Roles = "Counselor")]
        public async Task<ActionResult> CcaInterface()
        {
            // Look up counselor associated with this user
            var userId = User.Identity.GetUserId();
            var counselor = await db.Counselors.FirstOrDefaultAsync(m => m.UserId == userId).ConfigureAwait(false);
            List<CCA> ccas;
            // Look up all ccas associated with this primary
            if (counselor != null)
            {
                if (counselor.EnrollmentLocationID == GlobalVariables.PRIVATESCHOOLID)
                {
                    if (counselor.EnrollmentLocationSchoolNameID == null || counselor.EnrollmentLocationSchoolNameID == 0)
                    {
                        var studentIds = db.Students.Where(m => m.SchoolOfRecord.ToUpper() == counselor.School.ToUpper()).Select(m => m.UserId);
                        ccas = await db.CCAs.Where(m => studentIds.Contains(m.UserId)).ToListAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        ccas = await db.CCAs.Where(m => m.EnrollmentLocationSchoolNamesID == counselor.EnrollmentLocationSchoolNameID).ToListAsync().ConfigureAwait(false);
                    }
                }
                else
                {
                     ccas = await db.CCAs.Where(m => m.EnrollmentLocationSchoolNamesID == counselor.EnrollmentLocationSchoolNameID).ToListAsync().ConfigureAwait(false);
                }

                // Create list of viewmodels populated from 
                var ccaVmList = await GetCcaViewModelList(ccas).ConfigureAwait(false);

                ViewBag.SchoolName = await cactus.CactusSchools.Where(m => m.ID == counselor.EnrollmentLocationSchoolNameID).Select(m => m.Name).FirstOrDefaultAsync().ConfigureAwait(false);

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

                ccaVm.Session = await db.Session.FindAsync(ccaVm.SessionID).ConfigureAwait(false);
                ccaVm.OnlineCourse = await db.Courses.FindAsync(ccaVm.OnlineCourseID).ConfigureAwait(false);
                ccaVm.Provider = await db.Providers.FindAsync(ccaVm.ProviderID).ConfigureAwait(false);
                ccaVm.Student = await db.Students.FindAsync(ccaVm.StudentID).ConfigureAwait(false);
                ccaVm.CourseCredit = await db.CourseCredits.FindAsync(ccaVm.CourseCreditID).ConfigureAwait(false);
                ccaVm.CompletionStatus = await db.CourseCompletionStatus.FindAsync(cca.CourseCompletionStatusID).ConfigureAwait(false);
                ccaVm.CcaID = cca.ID;

                ccaVmList.Add(ccaVm);

            }
            return ccaVmList;
        }


        // GET: Counselors/Details/5
        [Authorize(Roles="Counselor,Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Counselor counselor = await db.Counselors.FindAsync(id).ConfigureAwait(false);
            if (counselor == null)
            {
                return HttpNotFound();
            }
            return View(counselor);
        }


        // GET: Counselors/Create
        public async Task<ActionResult> Create()
        {
            var leas = await cactus.CactusInstitutions.ToListAsync().ConfigureAwait(false);

            SetUpCreateView(leas);

            return View();
        }

        private void SetUpCreateView(List<CactusInstitution> leas)
        {
            leas.Insert(0, new CactusInstitution() { Name = "PRIVATE SCHOOL", ID = GlobalVariables.PRIVATESCHOOLID });
            leas.Insert(0, new CactusInstitution() { Code = "", Name = "District", ID = 0 });


            ViewBag.EnrollmentLocationID = new SelectList(leas, "ID", "Name");

            ViewBag.EnrollmentLocationSchoolNameID = new List<SelectListItem>();
            ViewBag.CounselorID = new List<SelectListItem>();
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
                    counselor = await db.Counselors.FindAsync(counselorVm.CounselorID).ConfigureAwait(false);
                }

                // Mapper causes all properties in the ViewModel to update the Counselor object so errors or not required empty properties will overwrite
                // properties in the database. (CactusID is one known problem.)
                // TODO: write own mapper type method that will check for nulls or known errors.

                if ((counselorVm.CactusID == 0 || counselorVm.CactusID == null) && counselor.CactusID != null)
                    counselorVm.CactusID = counselor.CactusID;

                counselor = Mapper.Map<CounselorViewModel, Counselor>(counselorVm, counselor);

                counselor.UserId = User.Identity.GetUserId();;

                // Remove all non numeric chars
                counselor.Phone = JustDigits(counselor.Phone);


                // If counselor not found in database need to add
                if (counselor.ID == 0)
                    db.Counselors.Add(counselor);


                var count = await db.SaveChangesAsync();

                if (count != 0) // Set account setup to true if successfully added
                {
                    var user = db.Users.Find(counselor.UserId);
                    user.IsSetup = true;
                    await db.SaveChangesAsync().ConfigureAwait(false);
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

            var leas = await cactus.CactusInstitutions.ToListAsync().ConfigureAwait(false);
            SetUpCreateView(leas);
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

        public JsonResult GetPrivateSchoolCounselors(string schoolName)
        {
            var counselors = db.Counselors.Where(m => m.School.Equals(schoolName)).Select(f => new SelectListItem
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
            var counselor = await db.Counselors.FirstOrDefaultAsync(c => c.ID == counselorId).ConfigureAwait(false);

            return Json(counselor);
        }

        // GET: Counselors/Edit/5
        [Authorize(Roles = "Counselor,Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Counselor counselor = await db.Counselors.FindAsync(id).ConfigureAwait(false);
            if (counselor == null)
            {
                return HttpNotFound();
            }

            await SetUpCounselorEdit(counselor);
            return View(counselor);
        }

        private async Task SetUpCounselorEdit(Counselor counselor)
        {
            
            var leas = await cactus.CactusInstitutions.ToListAsync();
            var schools = await cactus.CactusSchools.OrderByDescending(m => m.ID).ToListAsync().ConfigureAwait(false);
           
            leas.Insert(0, new CactusInstitution() { Name = "PRIVATE SCHOOL", ID = GlobalVariables.PRIVATESCHOOLID });
            leas.Insert(0, new CactusInstitution() { Code = "", Name = "District", ID = 0 });

            if (counselor.EnrollmentLocationID == null)
            {
                counselor.EnrollmentLocationID = schools.Where(m => m.ID == counselor.EnrollmentLocationSchoolNameID).Select(m => m.District).FirstOrDefault();
            }


            ViewBag.EnrollmentLocationID = new SelectList(leas, "ID", "Name", counselor.EnrollmentLocationID);

            if (counselor.EnrollmentLocationID == GlobalVariables.PRIVATESCHOOLID)
            {
                schools = schools.Where(m => m.SchoolType == GlobalVariables.PRIVATESCHOOLTYPE).ToList();
            }
            else
            {
                //schools = schools.Where(m => m.District == counselor.EnrollmentLocationID).ToList();
                schools.RemoveAll(m => m.Name == null);
                var distinctSchoolList = schools.GroupBy(x => x.Name).Select(group => group.First());
                schools = distinctSchoolList.Where(m => m.District == counselor.EnrollmentLocationID && !m.SchoolType.ToLower().Contains("dist")).OrderBy(m => m.Name).Distinct().ToList();
                
            }

            schools.Insert(0, new CactusSchool() { Name = "NOT LISTED", ID = 0 });
            ViewBag.EnrollmentLocationSchoolNameID = new SelectList(schools, "ID", "Name", counselor.EnrollmentLocationSchoolNameID);
        }

        // POST: Counselors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Counselor,Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserId,CactusID,Email,FirstName,LastName,Phone,School,EnrollmentLocationID,EnrollmentLocationSchoolNameID")] Counselor counselor)
        {
            if (ModelState.IsValid)
            {
                counselor.Phone = JustDigits(counselor.Phone);

                db.Entry(counselor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await SetUpCounselorEdit(counselor);
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
            Counselor counselor = await db.Counselors.FindAsync(id).ConfigureAwait(false);
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
            Counselor counselor = await db.Counselors.FindAsync(id).ConfigureAwait(false);
            db.Counselors.Remove(counselor);
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
