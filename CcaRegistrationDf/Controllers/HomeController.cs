using System.Linq;
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
                        var setup = await db.Students.Where(m => m.UserId == userId).FirstOrDefaultAsync();
                        if (setup == null)
                        {
                            return RedirectToAction("Index", "Students");
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