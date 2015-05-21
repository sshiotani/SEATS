﻿using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Threading.Tasks;
using CcaRegistrationDf.Models;
using System;


namespace CcaRegistrationDf.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Checks user setup status, if user exists in the student tables the account is
        /// setup.  
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            try
            {
                if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
                {

                    var userId = User.Identity.GetUserId();
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {

                        var setup = await db.Users.Where(m => m.Id == userId).Select(m => m.IsSetup).FirstOrDefaultAsync();
                        if (!setup)
                        {
                            return RedirectToAction("UserType");
                        }
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error in account verification. Error: " + ex.Message;
                return View("Error");
            }
        }

        //Get
        public ActionResult UserType()
        {
            try
            {

                var model = new UserTypeViewModel();
                model.UserType = db.Locations.Select(f => new SelectListItem()
                {
                    Value = f.ID.ToString(),
                    Text = f.Name
                });

                return View(model);

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Unable to get User Types. Error:" + ex.Message;
            }
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserType(UserTypeViewModel userType)
        {
            var type = await db.Locations.Where(m =>m.ID == userType.UserTypeID).FirstOrDefaultAsync();
            switch (type.Name)
            {
                case "Provider":
                    return RedirectToAction("Create", "ProviderUsers");
                case "Primary":
                    return RedirectToAction("Create", "PrimaryUsers");
                case "Counselor":
                    return RedirectToAction("Create", "Counselors");
                case "Student/Parent":
                    return RedirectToAction("Create", "Students");
                default:
                    break;
            }

            userType.UserType = db.Locations.Select(f => new SelectListItem()
                {
                    Value = f.ID.ToString(),
                    Text = f.Name
                });

            return View(userType);
        }

        public ActionResult About()
        {
            ViewBag.Message = "SEOP FAQ";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Information";

            return View();
        }
    }
}