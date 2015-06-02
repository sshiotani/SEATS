using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class CourseFee
    {
        public int ID { get; set; }

        // This is the fee for 1/2 credit all course fee 
        public decimal Fee { get; set; }

        // Fiscal Year this Fee is valid
        // public int FiscalYear { get; set; } 
     
    }
}