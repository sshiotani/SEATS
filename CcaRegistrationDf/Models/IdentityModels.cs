using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

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

       


        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.MainTable> MainTables { get; set; }

        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.Course> Courses { get; set; }

        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.OnlineProvider> OnlineProviders { get; set; }

        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.Session> Sessions { get; set; }

        public System.Data.Entity.DbSet<CcaRegistrationDf.Models.EnrollmentLocation> EnrollmentLocations { get; set; }
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