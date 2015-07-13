using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEATS.Models
{
    public class ProviderCcaViewModel
    {
        public DateTime ApplicationSubmissionDate { get; set; }
        public int FiscalYear { get; set; }
        public int CcaID { get; set; }
        public int ProviderID { get; set; }
        public Provider Provider { get; set; }
        public int StudentID { get; set; }
        public Student Student { get; set; }
        public int OnlineCourseID { get; set; }
        public OnlineCourse OnlineCourse { get; set; }
        public int CourseCreditID { get; set; }
        public CourseCredit CourseCredit { get; set; }
        public IEnumerable<SelectListItem> CourseCreditList { get; set; }
        public int CourseCategoryID { get; set; }
        public  CourseCategory CourseCategory { get; set; }
        public int SessionID { get; set; }
        public Session Session { get; set; }
        public Nullable<int> CourseCompletionStatusID { get; set; }
        public CourseCompletionStatus CourseCompletionStatus { get; set; }

        public string Primary { get; set; }

        [Display(Name = "Teacher Cactus ID")]
        public Nullable<int> TeacherCactusID { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        
        //public Nullable<System.DateTime> CourseBegin { get; set; }
        public Nullable<System.DateTime> CourseStartDate { get; set; }
        public Nullable<System.DateTime> CourseCompletionDate { get; set; }
        public Nullable<decimal> CreditCompletedToDate { get; set; }
        public Nullable<System.DateTime> DateConfirmationActiveParticipation { get; set; }
        public Nullable<System.DateTime> DateContinuationActiveParticipation { get; set; }
        public Nullable<System.DateTime> DateReportPassingGrade { get; set; }
        
        public bool IsProviderAcceptsRejectsCourseRequest { get; set; }
        //public bool IsProviderEnrollmentVerified { get; set; }
        public string ProviderSignature { get; set; }
        public bool IsProviderSignature { get; set; }
        public string ProviderExplanationRejection { get; set; }
        public Nullable<int> ProviderRejectionReasonsID { get; set; }
        public string ProviderNotes { get; set; }
    }
}