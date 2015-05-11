using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CcaRegistrationDf.Models
{
    public class CCAViewModel
    {
        public Nullable<int> SubmitterTypeID { get; set; }

        //Student
        public Nullable<int> StudentGradeLevel { get; set; }
        public bool HasExcessiveFED { get; set; }
        [Display(Name = "Why student is exceeding allowable online credits.")]
        public string ExcessiveFEDExplanation { get; set; }
        public Nullable<int> ExcessiveFEDReasonCode { get; set; }
        [Display(Name = "Additional Information or Comments")]
        public string Comments { get; set; }

        //Counselor
        public int CounselorID { get; set; }
        public virtual Counselor Counselor { get; set; }

        
        [EmailAddress]
        [Display(Name = "Counselor Email")]
        public string CounselorEmail { get; set; }

        
        [Display(Name = "Counselor First Name")]
        public string CounselorFirstName { get; set; }

        
        [Display(Name = "Counselor Last Name")]
        public string CounselorLastName { get; set; }

        [Display(Name = "Counselor Last Phone Number")]
        public string CounselorPhoneNumber { get; set; }


        //Provider
        [Display(Name = "Teacher Cactus ID")]
        public Nullable<int> TeacherCactusID { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }

        //Primary

        //Course
        public IEnumerable<SelectListItem> Session { get; set; }
        public int SessionID { get; set; }
        public IEnumerable<SelectListItem> CourseCategory { get; set; }
        public int CourseCategoryID { get; set; }
        public List<SelectListItem> Course { get; set; }
        public int CourseID { get; set; }
        
        public List<SelectListItem> CourseCredit { get; set; }
        public int CourseCreditID { get; set; }
        public int ProviderID { get; set; }
    }
}