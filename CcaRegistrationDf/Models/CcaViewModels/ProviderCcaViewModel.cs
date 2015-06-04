using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class ProviderCcaViewModel
    {
        public DateTime ApplicationSubmissionDate { get; set; }
        public int StudentID { get; set; }
        public int CcaID { get; set; }
        public int ProviderID { get; set; }
        public virtual Provider Provider { get; set; }
        [Display(Name = "Teacher Cactus ID")]
        public Nullable<int> TeacherCactusID { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public string CompletionStatus { get; set; }
        public Nullable<System.DateTime> CourseBegin { get; set; }
        public Nullable<System.DateTime> CourseStartDate { get; set; }
        public Nullable<System.DateTime> CourseCompletionDate { get; set; }
        public Nullable<decimal> CreditCompletedToDate { get; set; }
        public Nullable<System.DateTime> DateConfirmationActiveParticipation { get; set; }
        public Nullable<System.DateTime> DateContinuationActiveParticipation { get; set; }
        public Nullable<System.DateTime> DateReportPassingGrade { get; set; }
        public bool IsEnrollmentNoticeSent { get; set; }
        public bool IsProviderAcceptsRejectsCourseRequest { get; set; }
        public bool IsProviderEnrollmentVerified { get; set; }
        public bool IsProviderSignature { get; set; }
        public string ProviderExplanationRejection { get; set; }
        public string ProviderReasonRejection { get; set; }
    }
}