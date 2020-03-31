using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AirView.Entities
{
    /*----MoB!----*/
    public class ApiE911Status
    {
        public Int64 SiteId { get; set; }
        public Int64 NetworkModeId { get; set; }
        public Int64 BandId { get; set; }
        public Int64 CarrierId { get; set; }
        public Int64 ScopeId { get; set; }
        public Int64 SectorId { get; set; }
        public bool IsPerformed { get; set; }
        public string TesterName { get; set; }
        public Int64 TesterId { get; set; }
        public string Comment { get; set; }
    }
}
