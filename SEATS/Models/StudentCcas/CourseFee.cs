using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    public class CourseFee
    {
        public int ID { get; set; }

        // This is the fee for 1/2 credit all course fee 
        public decimal Fee { get; set; }

        // Fiscal Year this Fee is valid (ie for 2015-2016 ValidYear = 2016)
        public Nullable<int> ValidYear { get; set; } 
     
    }
}