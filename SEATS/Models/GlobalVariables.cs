using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    public static class GlobalVariables
    {
        public const short HOMESCHOOLID = 1; //LEA ID of a HOMESCHOOL student
        public const short PRIVATESCHOOLID = 2; // ... PRIVATESCHOOL
        public const string PRIVATESCHOOLTYPE = "PVSEC";
        public enum SubmitterTypeID
        {
            StudentParent,Counselor,Provider
        };
    }
}