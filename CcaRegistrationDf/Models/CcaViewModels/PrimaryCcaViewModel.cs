using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class PrimaryCcaViewModel
    {
        public DateTime ApplicationSubmissionDate { get; set; }
       
        public int CcaID { get; set; }
        public int StudentID { get; set; }
        public Student Student { get; set; }
        public int OnlineCourseID { get; set; }
        public OnlineCourse OnlineCourse { get; set; }
        public int CourseCreditID { get; set; }
        public CourseCredit CourseCredit { get; set; }

        public Nullable<int> EnrollmentLocationID { get; set; }


        public Nullable<int> ProviderID { get; set; }
        public Provider Provider { get; set; }

        public string BusinessAdministratorSignature { get; set; }
        public bool IsBusinessAdministratorAcceptRejectEnrollment { get; set; }
        public string PrimaryLEAExplantionRejection { get; set; }
        public string PrimaryLEAReasonRejectingCCA { get; set; }
        public string PrimaryNotes { get; set; }
    }
}