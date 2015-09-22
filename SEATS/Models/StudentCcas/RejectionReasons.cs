using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    public abstract class RejectionReasons
    {
        public int ID { get; set; }
        public string Reason { get; set; }
    }
}