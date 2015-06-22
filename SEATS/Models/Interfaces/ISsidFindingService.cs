using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SEATS.Models.Interfaces
{
    public interface ISsidFindingService
    {
        Task<string> GetSsid(StudentViewModel student);

        Task<List<SsidRecord>> GetSsids(StudentViewModel student);
    }
}