﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CcaRegistrationDf.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SEATSEntities : DbContext
    {
        public SEATSEntities()
            : base("name=SEATSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseCredit> CourseCredits { get; set; }
        public virtual DbSet<EnrollmentLocation> EnrollmentLocations { get; set; }
        public virtual DbSet<MainTable> MainTables { get; set; }
        public virtual DbSet<OnlineProvider> OnlineProviders { get; set; }
        public virtual DbSet<SchoolYearSession> SchoolYearSessions { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<SubmitterType> SubmitterTypes { get; set; }
    }
}
