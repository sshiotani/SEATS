using CcaRegistrationDf.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Student> Students { get; set; }
        DbSet<Parent> Parents { get; set; }
        DbSet<OnlineCourse> Courses { get; set; }
        DbSet<Provider> Providers { get; set; }
        DbSet<Counselor> Counselors { get; set; }
        DbSet<Session> Session { get; set; }
        DbSet<CourseCategory> CourseCategories { get; set; }
        DbSet<CourseCredit> CourseCredits { get; set; }
        DbSet<CCA> CCAs { get; set; }
        DbSet<ExcessiveFEDReason> ExcessiveFEDReasons { get; set; }
        DbSet<StudentBudget> StudentBudgets { get; set; }
        DbSet<CourseFee> CourseFees { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<ProviderUser> ProviderUsers { get; set; }
        DbSet<PrimaryUser> PrimaryUsers { get; set; }
        //DbSet<ApplicationUser> Users { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}