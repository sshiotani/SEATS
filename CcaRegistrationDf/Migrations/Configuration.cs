namespace CcaRegistrationDf.Migrations
{
    using CcaRegistrationDf.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CcaRegistrationDf.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
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
            this.AddUserAndRoles();
        }

        bool AddUserAndRoles()
        {
            bool success = false;

            var idManager = new IdentityManager();

            success = idManager.CreateRole("Admin");
            if (!success == true) return success;
            success = idManager.CreateRole("Support");
            if (!success == true) return success;
            success = idManager.CreateRole("Provider");
            if (!success == true) return success;
            success = idManager.CreateRole("Primary");
            if (!success == true) return success;
            success = idManager.CreateRole("Counselor");
            if (!success == true) return success;
            success = idManager.CreateRole("User");
            if (!success) return success;





            var newUser = new ApplicationUser()
            {
                UserName = "admin",
                Email = "edonline@schools.utah.gov",
                EmailConfirmed = true,
                IsSetup = true
            };



            // Be careful here - you  will need to use a password which will 

            // be valid under the password rules for the application, 

            // or the process will abort:

            success = idManager.CreateUser(newUser, "test@Account1");

            if (!success) return success;



            success = idManager.AddUserToRole(newUser.Id, "Admin");

            if (!success) return success;



            //success = idManager.AddUserToRole(newUser.Id, "Provider");

            //if (!success) return success;



            //success = idManager.AddUserToRole(newUser.Id, "User");

            //if (!success) return success;



            return success;

        }

    }
}
