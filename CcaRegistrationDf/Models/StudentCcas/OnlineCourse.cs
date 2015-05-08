using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class OnlineCourse
    {
        public int ID { get; set; }
        
        public string Name { get; set; }
        public string Credit { get; set; }
        public string Code { get; set; }
        
        public bool IsActive { get; set; }
        
        public string Notes { get; set; }
        public int CategoryID { get; set; }
        public int OnlineProviderID { get; set; }
        public int SessionID { get; set; }

        public virtual Category Category { get; set; }
        public virtual OnlineProvider OnlineProvider { get; set; }
        public virtual Session Session { get; set; }
       
    }
}