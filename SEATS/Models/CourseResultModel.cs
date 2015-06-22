using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEATS.Models
{
    public class CourseResultModel
    {
        public string Name { get; set; }
        public string Credit { get; set; }
        public string Code { get; set; }
        public int OnlineProviderID { get; set; }
        public string Notes { get; set; }

        public IEnumerable<SelectListItem> CreditChoices { get; set; }

    }
}