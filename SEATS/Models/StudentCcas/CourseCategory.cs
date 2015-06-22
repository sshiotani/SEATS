using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    public class CourseCategory
    {
        public CourseCategory()
        {
            this.Courses = new HashSet<OnlineCourse>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int CourseFeeID { get; set; }
        public virtual CourseFee CourseFee { get; set; }
    
        public virtual ICollection<OnlineCourse> Courses { get; set; }
    }
}