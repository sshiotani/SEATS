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
using CcaRegistrationDf.DAL;



namespace CcaRegistrationDf.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reports
        public async Task<ActionResult> Index()
        {
            var ccas = await db.CCAs.ToListAsync();

            Mapper.CreateMap<CCA, ReportViewModel>();
            List<ReportViewModel> reports = new List<ReportViewModel>();
            ReportViewModel report;

            foreach (var cca in ccas)
            {
                
                report =  Mapper.Map<CCA, ReportViewModel>(cca);
                report.Category = await db.CourseCategories.Where(m => m.ID == cca.CourseCategoryID).Select(m => m.Name).FirstAsync();
                report.Course = await db.Courses.Where(m => m.ID == cca.OnlineCourseID).Select(m => m.Name).FirstAsync();
                //report.OnlineProvider = await db.OnlineProviders.Where(m)

                
            }

            return View(reports);
        }
    }
}