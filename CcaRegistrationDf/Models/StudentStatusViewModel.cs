using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CcaRegistrationDf.Models
{
	public class StudentStatusViewModel
	{
		public DateTime ApplicationSubmissionDate { get; set; }
		public string CompletionStatus { get; set; }

		public virtual CourseCategory Category { get; set; }
		public virtual Provider Provider { get; set; }
		public virtual Session Session { get; set; }
		public virtual OnlineCourse OnlineCourse { get; set; }
	}
}