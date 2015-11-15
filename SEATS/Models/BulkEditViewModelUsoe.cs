using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEATS.Models
{
    public class BulkEditViewModelUsoe
    {
        public int CcaId { get; set; }
        public Student Student { get; set; }
        public int CourseCompletionStatusID { get; set; }
        public CourseCompletionStatus CourseCompletionStatus { get; set; }
        public IEnumerable<SelectListItem> CourseCompletionStatusList { get; set; }
        public int[] RowIds { get; set; }
        public bool IsEnrollmentNoticeSent { get; set; }
        public Nullable<System.DateTime> NotificationDate { get; set; }
        public Nullable<int> TeacherCactusID { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public string Notes { get; set; }
        public string RecordNotes { get; set; }
        public Nullable<decimal> BudgetPrimaryProvider { get; set; }
        public Nullable<decimal> PriorDisbursementProvider { get; set; }
        public Nullable<System.DateTime> RemediationPeriodBegins { get; set; }
        public Nullable<decimal> TotalDisbursementsProvider { get; set; }
        public Nullable<decimal> Offset { get; set; }
        public Nullable<decimal> Distribution { get; set; }
        public Nullable<decimal> Grand_Total { get; set; }

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
        public IEnumerable<SelectListItem> OnlineCourseList{ get; set; }
        public bool IsUpload { get; set; }
    }
}