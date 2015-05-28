using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class PrimaryCcaViewModel
    {
        public DateTime ApplicationSubmissionDate { get; set; }
        public int CCAID { get; set; }

        public Nullable<int> EnrollmentLocationID { get; set; }

        public Nullable<int> PrimaryID { get; set; }
        public virtual Primary Primary { get; set; }

        public bool IsBusinessAdministratorAcceptRejectEnrollment { get; set; }
        public string PrimaryLEAExplantionRejection { get; set; }
        public string PrimaryLEAReasonRejectingCCA { get; set; }

    }
}