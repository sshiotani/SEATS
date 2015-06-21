using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CcaRegistrationDf.Models
{
    /// <summary>
    /// This class is used to set the usertype when the user first signs up for an account.
    /// </summary>
    public class UserTypeViewModel
    {
        public int UserTypeID { get; set; }
        public Location User { get; set; }
    }
}