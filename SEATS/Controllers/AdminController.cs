using AutoMapper;
using SEATS.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web.Mvc;


namespace SEATS.Controllers
{
    /// <summary>
    /// This controller is used by the admin role to view CCAs.  The view associated with this controller contains link to other admin tools.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
    
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
                // Look up all CCAs and map them to viewmodel
                var ccaList = new List<UsoeCcaViewModel>();

                Mapper.CreateMap<CCA, UsoeCcaViewModel>();

                var ccas = await db.CCAs.ToListAsync();

                foreach (var cca in ccas)
                {
                    var ccaVm = Mapper.Map<CCA, UsoeCcaViewModel>(cca);
                    ccaVm.CcaID = cca.ID;

                    // Switch is slightly faster than if-else at this time.

                    switch (cca.EnrollmentLocationID)
                    {
                        case null:
                            break;
                        case GlobalVariables.HOMESCHOOLID:
                            ccaVm.Primary = "HOMESCHOOL";
                            break;
                        case GlobalVariables.PRIVATESCHOOLID:
                            ccaVm.Primary = "PRIVATESCHOOL";
                            break;
                        default:
                            var primary = await cactus.CactusInstitutions.FirstOrDefaultAsync(c => c.ID == cca.EnrollmentLocationID).ConfigureAwait(false);
                            if (primary != null)
                                ccaVm.Primary = primary.Name;
                            else
                                ccaVm.Primary = "UNKNOWN";
                            break;
                    }

                    //if (cca.EnrollmentLocationID == GlobalVariables.HOMESCHOOLID)
                    //    ccaVm.Primary = "HOMESCHOOL";
                    //else if (cca.EnrollmentLocationID == GlobalVariables.PRIVATESCHOOLID)
                    //    ccaVm.Primary = "PRIVATESCHOOL";
                    //else
                    //{
                    //    var primary = await cactus.CactusInstitutions.FirstOrDefaultAsync(c=>c.ID == cca.EnrollmentLocationID).ConfigureAwait(false);
                    //    if (primary != null)
                    //        ccaVm.Primary = primary.Name;
                    //    else
                    //        ccaVm.Primary = "UNKNOWN";
                    //}

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