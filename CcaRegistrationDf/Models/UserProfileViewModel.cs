using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CcaRegistrationDf.Models
{
    public class UserProfileViewModel
    {
        public int ID { get; set; }
        public string UserId { get; set; }  // GUID of the user
        public string Email { get; set; }
        public int UserLocationID { get; set; } // ID of the location in the provider or primary tables
        public string UserLocationName { get; set; }
        public IEnumerable<SelectListItem> UserLocation
        {
            get;
            set;
        }

        public int DistrictID { get; set; }
        public IEnumerable<SelectListItem> District
        {
            get;
            set;
        }

        public int LocationID { get; set; } // ID of the location type
        public virtual Location LocationType { get; set; }
        public IEnumerable<SelectListItem> Location
        {
            get;
            set;
        }
        
    }
}