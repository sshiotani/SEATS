using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class Student
    {
        public int ID { get; set; }

        public string UserId { get; set; }
        public Nullable<int> ParentID { get; set; }
        public virtual Parent Parent { get; set; }

        // Personal info
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }    
        public Nullable<int> SSID { get; set; }
        public Nullable<System.DateTime> StudentDOB { get; set; }
        public string StudentEmail { get; set; }

        //Enrollment info
        public Nullable<int> EnrollmentLocationID { get; set; }
        public Nullable<int> EnrollmentLocationSchoolNamesID { get; set; }
        public string SchoolOfRecord { get; set; } // For private school students
        public Nullable<System.DateTime> GraduationDate { get; set; }
        public bool HasHomeSchoolRelease { get; set; }


        //Other info
       
        public bool IsEarlyGraduate { get; set; }
        public bool IsFeeWaived { get; set; }
        public bool IsIEP { get; set; }
        public bool IsPrimaryEnrollmentVerified { get; set; }
        public bool IsSection504 { get; set; }
       
    }
}