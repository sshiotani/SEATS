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
        public HttpPostedFileBase File { get; set; }
    }
}