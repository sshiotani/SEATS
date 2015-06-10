using AutoMapper;
using CcaRegistrationDf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Interfaces;

namespace CcaRegistrationDf.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db;

        public AdminController()
        {
            this.db = new ApplicationDbContext();
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