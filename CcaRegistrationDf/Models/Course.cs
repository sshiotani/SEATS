//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CcaRegistrationDf.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Course
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Credit { get; set; }
        public string Code { get; set; }
        public int OnlineProviderID { get; set; }
        public int SessionID { get; set; }
        public bool IsActive { get; set; }
        
    
        public virtual Category Category { get; set; }
        public virtual OnlineProvider OnlineProvider { get; set; }
        public virtual Session Session { get; set; }
    }
}