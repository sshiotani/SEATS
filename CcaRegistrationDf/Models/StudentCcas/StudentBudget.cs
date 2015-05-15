using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class StudentBudget
    {
        public int ID { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public decimal OffSet { get; set; }
        public decimal Distribution { get; set; }

    }

 
}