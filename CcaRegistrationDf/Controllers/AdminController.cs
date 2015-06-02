using AutoMapper;
using CcaRegistrationDf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace CcaRegistrationDf.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: CCAs for Admin
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CcaInterface()
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();

                // Look up all CCAs
                var ccaList = new List<UsoeCcaViewModel>();

                Mapper.CreateMap<CCA, UsoeCcaViewModel>();
                var ccas = await db.CCAs.ToListAsync();

                foreach (var cca in ccas)
                {

                    var ccaVm = Mapper.Map<CCA, UsoeCcaViewModel>(cca);

                    ccaVm.Student = await db.Students.Where(m => m.ID == ccaVm.StudentID).FirstOrDefaultAsync().ConfigureAwait(false);
                    ccaVm.OnlineCourse = await db.Courses.Where(m => m.ID == ccaVm.OnlineCourseID).FirstOrDefaultAsync().ConfigureAwait(false);
                    ccaVm.CcaID = cca.ID;

                    ccaList.Add(ccaVm);

                }

                // Send to form to edit these ccas
                return View(ccaList);

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Unable to retrieve list of CCAs from database. Error:" + ex.Message;
                return View("Error");
            }

        }

    }
}