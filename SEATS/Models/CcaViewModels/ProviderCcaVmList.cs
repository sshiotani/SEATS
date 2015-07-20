using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    public class ProviderCcaVmList
    {
        public BulkEditViewModel BulkEdit { get; set; }
        public List<ProviderCcaViewModel> CcaList { get; set; }
    }
}