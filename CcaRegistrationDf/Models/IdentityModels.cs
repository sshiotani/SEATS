using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CcaRegistrationDf.Models
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

        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.Student> Students { get; set; }
        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.Parent> Parents { get; set; }
        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.OnlineCourse> Courses { get; set; }
        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.Provider> Providers { get; set; }
        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.Primary> Primaries { get; set; }
        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.Counselor> Counselors { get; set; }
        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.Session> Session { get; set; }
        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.CourseCategory> CourseCategories { get; set; }
        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.CourseCredit> CourseCredits { get; set; }
        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.CCA> CCAs { get; set; }

        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.ExcessiveFEDReason> ExcessiveFEDReasons { get; set; }

        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.StudentBudget> StudentBudgets { get; set; }

        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.CourseFee> CourseFees { get; set; }

        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.Location> Locations { get; set; }

        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.UserProfile> UserProfiles { get; set; }

        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.ProviderUser> ProviderUsers { get; set; }

        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.StudentCcas.PrimaryUser> PrimaryUsers { get; set; }

        
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


        public void ClearUserRoles(string userId)
        {

            var um = new UserManager<ApplicationUser>(

                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = um.FindById(userId);

            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.Roles);

            foreach (var role in currentRoles)
            {

                um.RemoveFromRole(userId, role.RoleId);

            }

        }

    }
}