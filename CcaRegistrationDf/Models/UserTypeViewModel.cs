using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CcaRegistrationDf.Models
{
    public class UserTypeViewModel
    {
        public int UserTypeID { get; set; }
        public Location User { get; set; }
        public IEnumerable<SelectListItem> UserType { get; set; }
    }
}