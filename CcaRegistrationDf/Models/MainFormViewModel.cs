﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CcaRegistrationDf.Models
{
    public class MainFormViewModel
    {

        public int SubmitterTypeID { get; set; }
        // Student info
        // public string Role { get; set; }

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
        [Range(7, 12)]
        public int StudentGradeLevel { get; set; }

        [Required]
        [Display(Name = "Student Last Name")]
        public string StudentLastName { get; set; }

        public Nullable<int> SSID { get; set; }

        public Nullable<DateTime> GraduationDate { get; set; }

        [Display(Name="Enrolled School District")]
        [Required]
        public int EnrollmentLocationID { get; set; }
        public IEnumerable<SelectListItem> EnrollmentLocation
        {
            get;
            set;
        }

        public string SchoolOfRecord { get; set; }
        public int EnrollmentLocationSchoolNamesID { get; set; }
        public List<SelectListItem> EnrollmentLocationSchoolNames { get; set; }
        public bool IsStudentSigned { get; set; }

        // ParentInfo
        [Required]
        [EmailAddress]
        [Display(Name = "Guardian Email")]
        public string GuardianEmail { get; set; }

        [Required]
        public string GuardianFirstName { get; set; }

        [Required]
        public string GuardianLastName { get; set; }
        public string GuardianPhone1 { get; set; }
        public string GuardianPhone2 { get; set; }
        //public bool IsGuardianSigned { get; set; }

        //Counselor info
        [Display(Name = "Counselor Cactus ID")]
        public Nullable<int> CounselorCactusID { get; set; }
        public string CounselorEmail { get; set; }
        public string CounselorFirstName { get; set; }
        public string CounselorLastName { get; set; }
        public string CounselorPhoneNumber { get; set; }
        public bool IsCounselorSigned { get; set; }

        // Course choice
        public IEnumerable<SelectListItem> Semester { get; set; }
        public int SemesterID { get; set; }
        public IEnumerable<SelectListItem> CourseCategory { get; set; }
        public int CourseCategoryID { get; set; }
        public List<SelectListItem> CourseName { get; set; }
        public int CourseNameID { get; set; }
        
        public List<SelectListItem> CourseCredit { get; set; }
        public int CourseCreditID { get; set; }
        public int OnlineProviderID { get; set; }

        // Student Accomodations
       
        
        public bool IsCourseConsistentWithStudentSEOP { get; set; }
        [Display(Name = "Student plans to graduate early.")]
        public bool IsEarlyGraduate { get; set; }
        [Display(Name = "Student has a fee waiver.")]
        public bool IsFeeWaived { get; set; }
        [Display(Name = "Student requires a Section 504 accomodation.")]
        public bool IsSection504 { get; set; }
        [Display(Name="Student has a Individual Education Plan (IEP).")]
        public bool IsIEP { get; set; }
        [Display(Name = "Additional Information or Comments")]
        public string Comments { get; set; }

        // Credit Exceptions

        public bool HasExcessiveFED { get; set; }
        [Display(Name = "Why student is exceeding allowable online credits.")]
        public string ExcessiveFEDExplanation { get; set; }
        public Nullable<int> ExcessiveFEDReasonCode { get; set; }
        public List<SelectListItem> ExcessiveFEDReasonList { get; set; }

        // Provider Section

        [Display(Name = "Teacher Cactus ID")]
        public Nullable<int> ProviderCactusID { get; set; }
        public string ProviderEmail { get; set; }
        public string ProviderFax { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderLastName { get; set; }
        public string ProviderPhoneNumber { get; set; }
        public bool IsProviderSignature { get; set; }
    }
}