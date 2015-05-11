using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public partial class Provider
    {
        
        public Provider()
        {
            this.OnlineCourses = new HashSet<OnlineCourse>();
        }
    
        
        public int ID { get; set; }
        public string Name { get; set; }
        public string DistrictID { get; set; }
        public bool IsActive { get; set; }
        public string ProviderEmail { get; set; }
       
        public string ProviderFax { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderLastName { get; set; }
        public string ProviderPhoneNumber { get; set; }
        
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public Nullable<int> TeacherCactusID { get; set; }

    
        public virtual ICollection<OnlineCourse> OnlineCourses { get; set; }
      
      
    }
}