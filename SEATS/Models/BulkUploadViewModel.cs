using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    public class BulkUploadViewModel
    {
        [Required]
        [Display(Name = "Excel File Name")]
        public HttpPostedFileBase File { get; set; }
    }
}