using CcaRegistrationDf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using AutoMapper;

using Microsoft.AspNet.Identity;

using System.Data.Entity;

using System.Net;



namespace CcaRegistrationDf.Controllers
{
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Reports
        public async Task<ActionResult> Index()
        {
            var ccas = await db.MainTables.ToListAsync();

            Mapper.CreateMap<MainTable, ReportViewModel>();
            List<ReportViewModel> reports = new List<ReportViewModel>();
            ReportViewModel report;

            foreach (var cca in ccas)
            {
                
                report =  Mapper.Map<MainTable, ReportViewModel>(cca);
                report.Category = await db.Categories.Where(m => m.ID == cca.CourseCategoryID).Select(m => m.Name).FirstAsync();
                report.Course = await db.Courses.Where(m => m.ID == cca.CourseNameID).Select(m => m.Name).FirstAsync();
                //report.OnlineProvider = await db.OnlineProviders.Where(m)

                
            }

            return View(reports);
        }
    }
}