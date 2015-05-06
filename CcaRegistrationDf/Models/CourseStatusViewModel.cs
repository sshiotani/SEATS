using System;
using System.Collections.Generic;

namespace CcaRegistrationDf.Models
{
    public class CourseStatusViewModel
    {
        public DateTime ApplicationSubmissionDate { get; set; }
        public string CompletionStatus { get; set; }

        public virtual Category Category { get; set; }
        public virtual OnlineProvider OnlineProvider { get; set; }
        public virtual Session Session { get; set; }
        public virtual Course Course { get; set; }
    }
}