using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    public class PrimaryUser
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }


        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        public int EnrollmentLocationID { get; set; }

        public int EnrollmentLocationSchoolNameID { get; set; }
        
    }
}