using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEATS.Models
{
    /// <summary>
    /// This model is used to hold information used to update multiple rows with one transaction.
    /// </summary>
    public class BulkEditViewModel
    {
      

        public int CcaId { get; set; }
        public Student Student { get; set; }
        public int CourseCompletionStatusID { get; set; }
        public CourseCompletionStatus CourseCompletionStatus { get; set; }
        public IEnumerable<SelectListItem> CourseCompletionStatusList { get; set; }
        public Nullable<System.DateTime> CourseStartDate { get; set; }
        public Nullable<System.DateTime> DateConfirmationActiveParticipation { get; set; }
        public Nullable<System.DateTime> CourseCompletionDate { get; set; }
        public Nullable<System.DateTime> DateContinuationActiveParticipation { get; set; }
        public Nullable<System.DateTime> DateReportPassingGrade { get; set; }
        public int[] RowIds { get; set; }

        //Added per SEATS-39
        public Nullable<int> TeacherCactusID { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public bool? IsProviderAcceptsRejectsCourseRequest { get; set; }
        public string ProviderExplanationRejection { get; set; }
        public Nullable<int> ProviderRejectionReasonsID { get; set; }
        public ProviderRejectionReasons ProviderRejectionReasons { get; set; }
        public IEnumerable<SelectListItem> ProviderRejectionReasonsList { get; set; }
        public string ProviderNotes { get; set; }

        //Course Added per SEATS-51
        public int SessionID { get; set; }
        public virtual Session Session { get; set; }
        public int CourseCategoryID { get; set; }
        public virtual CourseCategory CourseCategory { get; set; }
        public int OnlineCourseID { get; set; }
        public virtual OnlineCourse OnlineCourse { get; set; }
        public int CourseCreditID { get; set; }
        public virtual CourseCredit CourseCredit { get; set; }
        public IEnumerable<SelectListItem> CourseCreditList { get; set; }
        public IEnumerable<SelectListItem> SessionList { get; set; }
        public IEnumerable<SelectListItem> CourseCategoryList { get; set; }
        public IEnumerable<SelectListItem> OnlineCourseList { get; set; }
    }
}