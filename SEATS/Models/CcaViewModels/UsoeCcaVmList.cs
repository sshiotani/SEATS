using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEATS.Models
{
    public class UsoeCcaVmList
    {
        public BulkEditViewModelUsoe BulkEdit { get; set; }
        public List<UsoeCcaViewModel> CcaList { get; set; }
    }
}