using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    /// <summary>
    /// This class is used for the Provider Bulk Edit.  It contains a list of CCAs to update and what values need to be set.
    /// All the values that are set are set to the same value, thus the provider is able to update multiple records at once. (i.e. a 
    /// class will have all the same teacher, start date, etc.)
    /// </summary>
    public class ProviderCcaVmList
    {
        public BulkEditViewModel BulkEdit { get; set; }
        public List<ProviderCcaViewModel> CcaList { get; set; }
    }
}