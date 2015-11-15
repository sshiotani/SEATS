using AutoMapper;
using SEATS.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web.Mvc;
using System.Linq;

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

                var ccas = await db.CCAs.Where(m=>!m.IsUpload).ToListAsync();

                UsoeCcaVmList vmList = new UsoeCcaVmList();

                vmList.CcaList = await GetCcaViewModelList(ccas).ConfigureAwait(false);
                vmList.BulkEdit = new BulkEditViewModelUsoe();
                vmList.BulkEdit.SessionList = new SelectList(await db.Session.Where(m => m.Name != "All").ToListAsync().ConfigureAwait(false), "ID", "Name");
                vmList.BulkEdit.CourseCategoryList = new List<SelectListItem>();
                vmList.BulkEdit.OnlineCourseList = new List<SelectListItem>();
                vmList.BulkEdit.CourseCreditList = new List<SelectListItem>();

                var statusList = await db.CourseCompletionStatus.ToListAsync().ConfigureAwait(false); ;

                statusList.Insert(0, new CourseCompletionStatus { ID = 0, Status = "Select Status" });

                vmList.BulkEdit.CourseCompletionStatusList = statusList.Select(f => new SelectListItem
                {
                    Value = f.ID.ToString(),
                    Text = f.Status
                });

                // Send list of ccas to display
                return View(vmList);

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Unable to retrieve list of CCAs from database. Error:" + ex.Message;
                return View("Error");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CcaInterface(UsoeCcaVmList rowsToEdit)
        {

            TempData["RowsToEdit"] = rowsToEdit;

            // Send updated rows to cca controller 
            return RedirectToAction("SaveBulkUpdateUsoe", "CCAs");

        }

        // GET: CCAs for Admin
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CcaBudget()
        {
            try
            {
                // Look up all CCAs and map them to viewmodel
                var ccaList = new List<UsoeCcaViewModel>();

                Mapper.CreateMap<CCA, UsoeCcaViewModel>();

                var ccas = await db.CCAs.Where(m=>!m.IsUpload).ToListAsync();

                UsoeCcaVmList vmList = new UsoeCcaVmList();

                vmList.CcaList = await GetCcaViewModelList(ccas).ConfigureAwait(false);
                vmList.BulkEdit = new BulkEditViewModelUsoe();

                var statusList = await db.CourseCompletionStatus.ToListAsync().ConfigureAwait(false); ;

                statusList.Insert(0, new CourseCompletionStatus { ID = 0, Status = "Select Status" });

                vmList.BulkEdit.CourseCompletionStatusList = statusList.Select(f => new SelectListItem
                {
                    Value = f.ID.ToString(),
                    Text = f.Status
                });

                // Send list of ccas to display
                return View(vmList);

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Unable to retrieve list of CCAs from database. Error:" + ex.Message;
                return View("Error");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CcaBudget(UsoeCcaVmList rowsToEdit)
        {

            TempData["RowsToEdit"] = rowsToEdit;


            // Send updated rows to cca controller 
            return RedirectToAction("SaveBudgetUpdate", "CCAs");

        }

        /// <summary>
        /// Sets up ViewModel List for Bulk Edit
        /// </summary>
        /// <param name="ccas"></param>
        /// <returns></returns>
        private async Task<List<UsoeCcaViewModel>> GetCcaViewModelList(List<CCA> ccas)
        {
            Mapper.CreateMap<CCA, UsoeCcaViewModel>();

            var ccaVmList = new List<UsoeCcaViewModel>();


            foreach (var cca in ccas)
            {
                var ccaVm = Mapper.Map<CCA, UsoeCcaViewModel>(cca);
                ccaVm.CcaID = cca.ID;

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

                ccaVmList.Add(ccaVm);
            }

            return ccaVmList;

        }

       

    }
}