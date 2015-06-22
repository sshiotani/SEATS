using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    public class CounselorViewModel
    {
        [Display(Name = "Counselor Cactus ID")]
        public Nullable<int> CactusID { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Counselor Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Counselor First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Counselor Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Contact Phone Number")]
        public string Phone { get; set; }

        public int CounselorID { get; set; } // ID from Counselor table

        public string School { get; set; } // School name

        public int EnrollmentLocationID { get; set; } //ID from Cactus Institutions table

        public int EnrollmentLocationSchoolNameID { get; set; } // ID from Cactus School tables
    }
}