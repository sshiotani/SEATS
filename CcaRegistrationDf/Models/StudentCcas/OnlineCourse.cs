using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public partial class OnlineCourse
    {
        public int ID { get; set; }
        
        public string Name { get; set; }
        public string Credit { get; set; }
        public string Code { get; set; }
        
        public bool IsActive { get; set; }
        
        public string Notes { get; set; }
        public int CourseCategoryID { get; set; }
        public int ProviderID { get; set; }
        public int SessionID { get; set; }

        public virtual CourseCategory CourseCategory { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual Session Session { get; set; }
       
    }
}