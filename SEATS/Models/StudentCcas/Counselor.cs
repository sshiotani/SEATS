﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    public class Counselor
    {
        public int ID { get; set; }

        public string UserId { get; set; }

        public Nullable<int> CactusID { get; set; }
     
        public string Email { get; set; }
 
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string School { get; set; } // School name      

        public Nullable<int> EnrollmentLocationID { get; set; } //ID from Cactus Institutions table 

        public Nullable<int> EnrollmentLocationSchoolNameID { get; set; } // ID from Cactus School tables
        
    }
}