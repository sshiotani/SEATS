using System;
using System.Collections.Generic;

namespace CcaRegistrationDf.Models
{
    public class CourseStatusViewModel
    {
        /// <summary>
        /// This class is used to list the CCA for student/parent viewing.
        /// </summary>
        public DateTime ApplicationSubmissionDate { get; set; }
        public string CompletionStatus { get; set; }

        public int CourseCategoryID { get; set; }
        public CourseCategory CourseCategory { get; set; }
        public int ProviderID { get; set; }
        public Provider Provider { get; set; }
        public int SessionID { get; set; }
        public Session Session { get; set; }
        public int OnlineCourseID { get; set; }
        public OnlineCourse OnlineCourse { get; set; }
    }
}