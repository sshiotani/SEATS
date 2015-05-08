using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class Primary
    {
        public int ID { get; set; }
        public string Business_Administrator_Email { get; set; }
        public string BusinessAdministratorFirstName { get; set; }
        public string BusinessAdministratorLastName { get; set; }
        public bool BusinessAdministratorSignature { get; set; }
        public string BusinessAdministratorTelephone { get; set; }
        public Nullable<System.DateTime> DateBusinessAdministratorSignature { get; set; }
      
    }
}