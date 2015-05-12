using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class Counselor
    {
        public int ID { get; set; }
        public Nullable<int> CounselorCactusID { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Counselor Email")]
        public string CounselorEmail { get; set; }

        [Required]
        [Display(Name = "Counselor First Name")]
        public string CounselorFirstName { get; set; }

        [Required]
        [Display(Name = "Student Last Name")]
        public string CounselorLastName { get; set; }

        public string CounselorPhoneNumber { get; set; }
        
    }
}