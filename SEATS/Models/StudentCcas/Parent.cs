using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    public class Parent
    {
        public int ID { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Guardian Email")]
        public string GuardianEmail { get; set; }

        [Required]
        [Display(Name = "Guardian First Name")]
        public string GuardianFirstName { get; set; }

        [Required]
        [Display(Name = "Guardian Last Name")]
        public string GuardianLastName { get; set; }


        public string GuardianPhone1 { get; set; }
        public string GuardianPhone2 { get; set; }
       
    }
}