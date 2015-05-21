using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class Counselor
    {
        public int ID { get; set; }
        public string UserId { get; set; }

        public Nullable<int> PersonID { get; set; }
        public Nullable<int> CounselorCactusID { get; set; }
     
        public string CounselorEmail { get; set; }
 
        public string CounselorFirstName { get; set; }

        public string CounselorLastName { get; set; }

        public string CounselorPhoneNumber { get; set; }

        public string School { get; set; }

        public Nullable<int> SchoolID { get; set; }
        
    }
}