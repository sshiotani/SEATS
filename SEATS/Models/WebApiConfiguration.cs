using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEATS.Models
{
    /// <summary>
    /// Holds url and authentification token for web api.  Injected from main Web.config during 
    /// service construction.
    /// </summary>
    public class WebApiConfiguration
    {
        public string ssidApiBase { get; set; }
        public string token { get; set; }
    }
}
