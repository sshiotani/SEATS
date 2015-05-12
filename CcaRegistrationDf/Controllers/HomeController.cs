using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Threading.Tasks;
using CcaRegistrationDf.Models;


namespace CcaRegistrationDf.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                using(ApplicationDbContext db = new ApplicationDbContext())
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

        public ActionResult About()
        {
            ViewBag.Message = "SEOP FAQ";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}