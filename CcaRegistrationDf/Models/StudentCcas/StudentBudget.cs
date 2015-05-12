using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public class StudentBudget
    {
        public int ID { get; set; }

        public int Year { get; set; }
        public Month Month { get; set; }
        public decimal OffSet { get; set; }
        public decimal Distribution { get; set; }

    }
}