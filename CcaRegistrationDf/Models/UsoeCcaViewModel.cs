﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class UsoeCcaViewModel
    {
        public DateTime ApplicationSubmissionDate { get; set; }

        public string UserId { get; set; }
        public int CCAID { get; set; }
        public int StudentID { get; set; }
        public Nullable<decimal> BudgetPrimaryProvider { get; set; }
        public bool IsRemediation { get; set; }
        public Nullable<System.DateTime> NotificationDate { get; set; }
        public Nullable<System.DateTime> PrimaryNotificationDate { get; set; }
        public Nullable<decimal> PriorDisbursementProvider { get; set; }
        public string RecordNotes { get; set; }
        public Nullable<System.DateTime> RemediationPeriodBegins { get; set; }
        public Nullable<decimal> TotalDisbursementsProvider { get; set; }
        public Nullable<System.DateTime> TwentyDaysPastSemesterStartDate { get; set; }
        public Nullable<decimal> Unallocated { get; set; }
        public Nullable<decimal> UnallocatedReduction { get; set; }
        public Nullable<System.DateTime> WithdrawalDate { get; set; }
        public Nullable<decimal> Grand_Total { get; set; }
        public string Notes { get; set; }
    }
}