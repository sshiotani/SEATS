﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CcaRegistrationDf.Models
{
    public class CourseResultModel
    {
        public string Name { get; set; }
        public string Credit { get; set; }
        public string Code { get; set; }

        public List<SelectListItem> CreditChoices { get; set; }

    }
}