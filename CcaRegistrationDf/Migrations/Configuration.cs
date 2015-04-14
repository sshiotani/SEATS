namespace CcaRegistrationDf.Migrations
{
    using CcaRegistrationDf.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CcaRegistrationDf.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "CcaRegistrationDf.Models.ApplicationDbContext";
        }

        protected override void Seed(CcaRegistrationDf.Models.ApplicationDbContext context)
        {

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            this.AddRoles();

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "admin",
                Email = "admin@seats.com",
                EmailConfirmed = true

            };

            manager.Create(user, "test@Account1");
            manager.AddToRole(user.Id, "Admin");
            manager.AddToRole(user.Id, "Provider");
            manager.AddToRole(user.Id, "User");



        }

        bool AddRoles()
        {
            bool success = false;

            var idManager = new IdentityManager();

            success = idManager.CreateRole("Admin");
            if (!success == true) return success;

            success = idManager.CreateRole("Provider");
            if (!success == true) return success;

            success = idManager.CreateRole("User");
            return success;



        }



    }
}
