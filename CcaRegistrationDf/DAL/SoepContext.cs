using CcaRegistrationDf.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.DAL
{
    public class SoepContext : DbContext
    {

        public SoepContext()
            : base("SoepContext")
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<OnlineCourse> Courses { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Primary> Primaries { get; set; }
        public DbSet<Counselor> Counselors { get; set; }
        public DbSet<Session> ClassSession { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<CourseCredit> CourseCredits { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}