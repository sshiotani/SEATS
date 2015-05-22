using System;
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
    [Authorize(Roles = "Admin")]
    public class UserProfilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserProfiles
        public async Task<ActionResult> Index()
        {
            var userProfiles = db.UserProfiles.Include(u => u.Location).Select(f =>
                new UserProfileViewModel()
                {
                    ID = f.ID,
                    LocationID = f.LocationID,
                    LocationType = f.Location,
                    UserId = f.UserId,
                    Email = db.Users.Where(u => u.Id == f.UserId).Select(u => u.Email).FirstOrDefault(),
                    UserLocationID = f.UserLocationID,
                    UserLocationName = f.UserLocationName
                });




            return View(await userProfiles.ToListAsync());
        }

        // GET: UserProfiles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfile userProfile = await db.UserProfiles.FindAsync(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            return View(userProfile);
        }

        // GET: UserProfiles/Create
        public ActionResult Create()
        {
            var manager = new SelectUserRolesViewModel();
            var roles = manager.Roles.Where(m => m.RoleName == "Provider" || m.RoleName == "Primary" ).Select(m => m.RoleId);
            var users = db.Users;

            ViewBag.UserId = new SelectList(users, "Id", "Email");

            var model = new UserProfileViewModel();
            model.District = new List<SelectListItem>();
            model.UserLocation = new List<SelectListItem>();
            model.Location = new SelectList(db.Locations, "ID", "Name");

            return View(model);
        }

        // POST: UserProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,UserId,UserLocationID,LocationID,UserLocationName")] UserProfileViewModel userProfileVm)
        {
            if (ModelState.IsValid)
            {
                var newProfile = await db.UserProfiles.Where(m => m.UserId == userProfileVm.UserId).FirstOrDefaultAsync().ConfigureAwait(false);
                if (newProfile != null)
                {
                    ModelState.AddModelError("", "Error! User exists in the User Profile database.  Delete user to complete transaction.");
                }
                else
                {
                    Mapper.CreateMap<UserProfileViewModel, UserProfile>();

                    UserProfile userProfile = Mapper.Map<UserProfileViewModel, UserProfile>(userProfileVm);

                    db.UserProfiles.Add(userProfile);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }


            userProfileVm.District = new List<SelectListItem>();
            userProfileVm.UserLocation = new List<SelectListItem>();
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");

            userProfileVm.Location = new SelectList(db.Locations, "ID", "Name");

            return View(userProfileVm);
        }

        public async Task<JsonResult> GetDistricts()
        {
            try
            {
                using (CactusEntities leas = new CactusEntities())
                {
                    var leaList = await leas.CactusInstitutions.OrderBy(m => m.Name).ToListAsync();

                    leaList.Insert(0, new CactusInstitution() { Name = "PRIVATE SCHOOL", DistrictID = 1.0M });

                    var locations = leaList.Select(f => new SelectListItem
                    {
                        Value = Decimal.ToInt32(f.DistrictID).ToString(),
                        Text = f.Name
                    });

                    return Json(new SelectList(locations, "Value", "Text"));
                }
            }
            catch
            {
                throw new HttpException(500, "Error processing request.");

            }
        }

        public async Task<JsonResult> GetProviders()
        {
            try
            {

                var providerList = await db.Providers.OrderBy(m => m.Name).ToListAsync();


                var locations = providerList.Select(f => new SelectListItem
                {
                    Value = f.ID.ToString(),
                    Text = f.Name
                });

                return Json(new SelectList(locations, "Value", "Text"));

            }
            catch
            {
                throw new HttpException(500, "Error processing request.");

            }
        }

        public async Task<JsonResult> GetSchools(int districtId)
        {
            try
            {
                using (CactusEntities leas = new CactusEntities())
                {
                    var district = Convert.ToDecimal(districtId);
                    var schoolList = await leas.CactusSchools.Where(m => m.district == district).OrderBy(m => m.name).ToListAsync();

                    var locations = schoolList.Select(f => new SelectListItem
                    {
                        Value = f.id.ToString(),
                        Text = f.name
                    });

                    return Json(new SelectList(locations, "Value", "Text"));
                }

            }
            catch
            {
                throw new HttpException(500, "Error processing request.");

            }
        }
        // GET: UserProfiles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfile userProfile = await db.UserProfiles.FindAsync(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }

            UserProfileViewModel userProfileVm = new UserProfileViewModel();

            userProfileVm.UserLocationName = userProfile.UserLocationName;
            userProfileVm.Email = await db.Users.Where(u => u.Id == userProfile.UserId).Select(m => m.Email).FirstOrDefaultAsync();

            return View(userProfileVm);
        }

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserId,UserLocationID,LocationID,UserLocationName")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userProfile).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.LocationID = new SelectList(db.Locations, "ID", "Name", userProfile.LocationID);
            return View(userProfile);
        }

        // GET: UserProfiles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfile userProfile = await db.UserProfiles.FindAsync(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            return View(userProfile);
        }

        // POST: UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UserProfile userProfile = await db.UserProfiles.FindAsync(id);
            db.UserProfiles.Remove(userProfile);
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
