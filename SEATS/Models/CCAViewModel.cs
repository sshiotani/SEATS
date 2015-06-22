using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEATS.Models
{
	public class CCAViewModel
	{

		public int SubmitterTypeID { get; set; }
        public string UserId { get; set; }

		// Student
		[Required]
		public Nullable<int> StudentGradeLevel { get; set; }
		public bool HasExcessiveFED { get; set; }
		public string ExcessiveFEDExplanation { get; set; }
		public Nullable<int> ExcessiveFEDReasonID { get; set; }
		public IEnumerable<SelectListItem> ExcessiveFEDReasonList { get; set; }
		public int EnrollmentLocationID { get; set; }
		

		//Counselor
		public Nullable<int> CounselorID { get; set; }
		public virtual Counselor Counselor { get; set; }
		public IEnumerable<SelectListItem> CounselorList { get; set; }
		public Nullable<int> CactusID { get; set; }
		public bool IsCounselorSigned { get; set; }

		
		[EmailAddress]
		[Display(Name = "Counselor Email")]
		public string CounselorEmail { get; set; }

	  
		[Display(Name = "Counselor First Name")]
		public string CounselorFirstName { get; set; }

		
		[Display(Name = "Student Last Name")]
		public string CounselorLastName { get; set; }

		public string CounselorPhoneNumber { get; set; }

		//Provider
		public int ProviderID { get; set; }
        //[Display(Name = "Teacher Cactus ID")]
        //public Nullable<int> TeacherCactusID { get; set; }
        //public string TeacherFirstName { get; set; }
        //public string TeacherLastName { get; set; }

		//Course
		public int OnlineCourseID { get; set; }
		public IEnumerable<SelectListItem> OnlineCourse { get; set; }
		public int CourseCategoryID { get; set; }
		public IEnumerable<SelectListItem> CourseCategory { get; set; }
		public int CourseCreditID { get; set; }
		public IEnumerable<SelectListItem> CourseCredit { get; set; }
		public int SessionID { get; set; }
		public IEnumerable<SelectListItem> Session { get; set; }
        public Nullable<decimal> CourseFee { get; set; }

		//Misc
		public string Comments { get; set; }
	}
}