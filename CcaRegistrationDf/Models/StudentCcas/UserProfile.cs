using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    /// <summary>
    /// This class is used to define where the the User is located and the type of location the user is at (Provider, Primary)
    /// </summary>
    public class UserProfile
    {
        public int ID { get; set; }
        public string UserId { get; set; }  // GUID of the user
        public int UserLocationID { get; set; } // ID of the location in the provider or primary tables
        public string UserLocationName { get; set; }

        public int LocationID { get; set; } // ID of the location type
        public virtual Location Location { get; set; } // Sets the Location type of the user (Provider, Primary)
        
    }
}