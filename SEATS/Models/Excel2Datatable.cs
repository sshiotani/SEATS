using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    public class Excel2Datatable
    {
        public DataTable table { get; set; }
        public List<string> readErrors { get; set; }
    }
}