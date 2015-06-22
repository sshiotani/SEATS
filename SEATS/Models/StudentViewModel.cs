using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SEATS.Models
{
    /// <summary>
    /// This View Model is used for the student account registration process.
    /// </summary>
    public class StudentViewModel
    {
        [Required]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<DateTime> StudentDOB { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Student Email")]
        public string StudentEmail { get; set; }

        [Required]
        [Display(Name = "Student First Name")]
        public string StudentFirstName { get; set; }


        [Required]
        [Display(Name = "Student Last Name")]
        public string StudentLastName { get; set; }

        [Display(Name = "Student Number")]
        public Nullable<int> StudentNumber { get; set; }

        [Required]
        [Display(Name = "Graduation Date")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> GraduationDate { get; set; }

        [Display(Name = "Enrolled School District")]
        [Required]
        public int EnrollmentLocationID { get; set; }
        public IEnumerable<SelectListItem> EnrollmentLocation
        {
            get;
            set;
        }

        public string SchoolOfRecord { get; set; }
        public Nullable<int> EnrollmentLocationSchoolNamesID { get; set; }
        public List<SelectListItem> EnrollmentLocationSchoolNames { get; set; }

        [Display(Name = "Student has a home school release.")]
        public bool HasHomeSchoolRelease { get; set; }

        // Credit Exceptions

        [Display(Name = "Student plans to graduate early.")]
        public bool IsEarlyGraduate { get; set; }
        [Display(Name = "Student has a fee waiver.")]
        public bool IsFeeWaived { get; set; }
        [Display(Name = "Student requires a Section 504 accomodation.")]
        public bool IsSection504 { get; set; }
        [Display(Name = "Student has a Individual Education Plan (IEP).")]
        public bool IsIEP { get; set; }
        [Display(Name = "Additional Information or Comments")]
        public string Comments { get; set; }

    }
}