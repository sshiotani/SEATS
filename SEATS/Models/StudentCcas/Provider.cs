using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    public class Provider
    {
        
        public Provider()
        {
            this.OnlineCourses = new HashSet<OnlineCourse>();
        }
        
        public int ID { get; set; }      
        public string Name { get; set; }
        public string DistrictNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<OnlineCourse> OnlineCourses { get; set; }
      
      
    }
}