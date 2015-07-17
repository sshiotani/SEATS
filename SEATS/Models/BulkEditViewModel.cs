using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    public class BulkEditViewModel
    {
        public int CcaId { get; set; }
        public Student Student { get; set; }
        public int CourseCompletionStatusID { get; set; }
        public CourseCompletionStatus CourseCompletionStatus { get; set; }
        public Nullable<System.DateTime> CourseStartDate { get; set; }
        public Nullable<System.DateTime> DateConfirmationActiveParticipation { get; set; }
        public Nullable<System.DateTime> CourseCompletionDate { get; set; }
        public int[] RowIds { get; set; }
    }
}