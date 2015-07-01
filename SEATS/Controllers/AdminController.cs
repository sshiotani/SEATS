using AutoMapper;
using SEATS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;


namespace SEATS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private const short HOMESCHOOLID = 1; //LEA ID of a HOMESCHOOL student
        private const short PRIVATESCHOOLID = 2; // ... PRIVATESCHOOL
        private ApplicationDbContext db { get; set; }
        private SEATSEntities cactus { get; set; }

        public AdminController()
        {
            this.db = new ApplicationDbContext();
            this.cactus = new SEATSEntities();
        }

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
                // Look up all CCAs
                var ccaList = new List<UsoeCcaViewModel>();

                Mapper.CreateMap<CCA, UsoeCcaViewModel>();

                var ccas = await db.CCAs.ToListAsync();

                foreach (var cca in ccas)
                {
                    var ccaVm = Mapper.Map<CCA, UsoeCcaViewModel>(cca);
                    ccaVm.CcaID = cca.ID;
                    if (cca.EnrollmentLocationID == HOMESCHOOLID)
                        ccaVm.Primary = "Homeschool";
                    else if (cca.EnrollmentLocationID == PRIVATESCHOOLID)
                        ccaVm.Primary = "Private";
                    else
                    {
                        var primary = await cactus.CactusInstitutions.FindAsync(cca.EnrollmentLocationID).ConfigureAwait(false);
                        ccaVm.Primary = primary.Name;
                    }

                    ccaList.Add(ccaVm);
                }

                // Send list of ccas to display
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