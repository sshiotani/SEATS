using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models.StudentCcas
{
    public enum Months
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    public partial class StudentBudget
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public int Year { get; set; }
        public Months Month { get; set; }
        public decimal OffSet { get; set; }
        public decimal Distribution { get; set; }
    }
}