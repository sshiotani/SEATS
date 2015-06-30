using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;


namespace SEATS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public bool IsSetup { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
       public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<OnlineCourse> Courses { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Counselor> Counselors { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<CourseCredit> CourseCredits { get; set; }
        public DbSet<CCA> CCAs { get; set; }
        public DbSet<ExcessiveFEDReason> ExcessiveFEDReasons { get; set; }
        public DbSet<StudentBudget> StudentBudgets { get; set; }
        public DbSet<CourseFee> CourseFees { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<ProviderUser> ProviderUsers { get; set; }
        public DbSet<PrimaryUser> PrimaryUsers { get; set; }
        public System.Data.Entity.DbSet<SEATS.Models.PrimaryRejectionReasons> PrimaryRejectionReasons { get; set; }

        public System.Data.Entity.DbSet<SEATS.Models.ProviderRejectionReasons> ProviderRejectionReasons { get; set; }

        public System.Data.Entity.DbSet<SEATS.Models.CourseCompletionStatus> CourseCompletionStatus { get; set; }

      
        
    }

    public class IdentityManager
    {

        public bool RoleExists(string name)
        {

            var rm = new RoleManager<IdentityRole>(

                new RoleStore<IdentityRole>(new ApplicationDbContext()));

            return rm.RoleExists(name);

        }

        public bool CreateRole(string name)
        {

            var rm = new RoleManager<IdentityRole>(

                new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var idResult = rm.Create(new IdentityRole(name));

            return idResult.Succeeded;

        }

        public bool CreateUser(ApplicationUser user, string password)
        {

            var um = new UserManager<ApplicationUser>(

                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var idResult = um.Create(user, password);

            return idResult.Succeeded;

        }

        public bool AddUserToRole(string userId, string roleName)
        {

            var um = new UserManager<ApplicationUser>(

                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var idResult = um.AddToRole(userId, roleName);

            return idResult.Succeeded;

        }


        public async Task ClearUserRoles(string userId)
        {

            var um = new UserManager<ApplicationUser>(

                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = um.FindById(userId);

            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.Roles);

            ApplicationDbContext db = new ApplicationDbContext();

          
            foreach (var role in currentRoles)
            {
                var roleName = db.Roles.Where(m => m.Id == role.RoleId).Select(m => m.Name).FirstOrDefault();

               var idsuccess = um.RemoveFromRole(userId, roleName);

            }

        }

    }
}