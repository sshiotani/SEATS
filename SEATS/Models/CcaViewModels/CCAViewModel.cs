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
        //public DateTime ApplicationSubmissionDate { get; set; }
        //public int FiscalYear { get; set; }
        //public int CcaID { get; set; }
        //public int StudentID { get; set; }
        //public Student Student { get; set; }
        //public Provider Provider { get; set; }
        //public OnlineCourse OnlineCourse { get; set; }
        //public CourseCredit CourseCredit { get; set; }
        //public CourseCategory CourseCategory { get; set; }
        //public Session Session { get; set; }
        //public Nullable<int> CourseCompletionStatusID { get; set; }
        //public CourseCompletionStatus CourseCompletionStatus { get; set; }

        public int SubmitterTypeID { get; set; }
        public string UserId { get; set; }
        
        public bool IsSubmittedByProxy { get; set; }

        // Student		
        public int? StudentGradeLevel { get; set; }
		public bool? HasExcessiveFED { get; set; }
		public string ExcessiveFEDExplanation { get; set; }
		public Nullable<int> ExcessiveFEDReasonID { get; set; }
		public IEnumerable<SelectListItem> ExcessiveFEDReasonList { get; set; }
		public int? EnrollmentLocationID { get; set; }
        public string SchoolOfRecord { get; set; }
        public bool EarlyGraduate { get; set; }

        //Counselor
        public Nullable<int> CounselorID { get; set; }
		public Counselor Counselor { get; set; }
		public IEnumerable<SelectListItem> CounselorList { get; set; }
		public Nullable<int> CactusID { get; set; }
		public bool IsCounselorSigned { get; set; }
        public bool IsCounselorRejecting { get; set; }
        public Nullable<int> CounselorRejectionReasonsID { get; set; }
        public CounselorRejectionReasons CounselorRejectionReasons { get; set; }
        public string CounselorRejectionExplantion { get; set; }


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
		public IEnumerable<SelectListItem> OnlineCourseList { get; set; }
		public int CourseCategoryID { get; set; }
		public IEnumerable<SelectListItem> CourseCategoryList { get; set; }
		public int CourseCreditID { get; set; }
		public IEnumerable<SelectListItem> CourseCreditList { get; set; }
		public int SessionID { get; set; }
		public IEnumerable<SelectListItem> SessionList { get; set; }
        public Nullable<decimal> CourseFee { get; set; }

        //Misc
        public Nullable<int> PrimaryRejectionReasonsID { get; set; }
        //public PrimaryRejectionReasons PrimaryRejectionReasons { get; set; }
        public Nullable<int> ProviderRejectionReasonsID { get; set; }
        //public ProviderRejectionReasons ProviderRejectionReasons { get; set; }
        public string Comments { get; set; }
	}
}