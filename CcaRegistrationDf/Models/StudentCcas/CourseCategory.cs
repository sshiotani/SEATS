﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public partial class CourseCategory
    {
        public CourseCategory()
        {
            this.OnlineCourses = new HashSet<OnlineCourse>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    
        public virtual ICollection<OnlineCourse> OnlineCourses { get; set; }
    }
}