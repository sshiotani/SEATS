﻿using CcaRegistrationDf.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.DAL
{
    //public class ApplicationDbContext : DbContext
    //{

    //    public ApplicationDbContext()
    //        : base("ApplicationDbContext")
    //    {
    //    }

    //    public DbSet<Student> Students { get; set; }
    //    public DbSet<Parent> Parents { get; set; }
    //    public DbSet<Name> Courses { get; set; }
    //    public DbSet<Provider> Providers { get; set; }
    //    public DbSet<Primary> Primaries { get; set; }
    //    public DbSet<Counselor> Counselors { get; set; }
    //    public DbSet<Session> ClassSession { get; set; }
    //    public DbSet<Name> CourseCategories { get; set; }
    //    public DbSet<CourseCredit> CourseCredits { get; set; }


    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    //    }

    //    public System.Data.Entity.DbSet<CcaRegistrationDf.Models.CCA> CCAs { get; set; }
    //}
}