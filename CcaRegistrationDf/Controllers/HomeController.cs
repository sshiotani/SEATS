using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Threading.Tasks;
using CcaRegistrationDf.Models;
using System;
using System.Collections.Generic;
using AutoMapper;


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
        public ActionResult Index()
        {
            try
            {
                // Check for account setup
                if (User.Identity.IsAuthenticated)
                {

                    var userId = User.Identity.GetUserId();


                    var user = db.Users.Find(userId);

                    var setup = user.IsSetup;

                    if (!setup)
                    {
                        return RedirectToAction("UserType");
                    }


                    return RedirectToAction("CheckRole");
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error in account verification. Error: " + ex.Message;
                return View("Error");
            }


        }

        /// <summary>
        /// This method redirects User depending on their role.  Right now we direct Outside Users (besides Student/Parents)
        /// to their CCA Interface.
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckRole()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (User.IsInRole("Primary"))
            {
                return RedirectToAction("CcaInterface", "PrimaryUsers");
            }
            else if (User.IsInRole("Provider"))
            {
                return RedirectToAction("CcaInterface", "ProviderUsers");
            }
            else if (User.IsInRole("Counselor"))
            {
                return RedirectToAction("CcaInterface", "Counselors");
            }

            return RedirectToAction("Index2");
        }

        public ActionResult Index2()
        {


            return View();



        }

        /// <summary>
        /// Gets user types to display in dropdown for Account setup.  User chooses which type of account wanted.
        /// </summary>
        /// <returns></returns>
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

        public ActionResult RequestSent()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserType(UserTypeViewModel userType)
        {
            var type = await db.Locations.FindAsync(userType.UserTypeID).ConfigureAwait(false);
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