using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEATS.Models
{
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
    }
}