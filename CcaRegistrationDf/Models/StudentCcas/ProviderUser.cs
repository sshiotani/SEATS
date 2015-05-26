using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class ProviderUser
    {
        public int ID { get; set; }

        public string UserId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Fax")]
        public string Fax { get; set; }

        public int ProviderID { get; set; }
        public virtual Provider Provider { get; set; }
    }
}