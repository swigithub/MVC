using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.Entities
{
   public class AV_Search
    {
        public Int64 SiteId { get; set; }
        public Int64 NetworkModeId { get; set; }
        public Int64 BandId { get; set; }
        public Int64 CarrierId { get; set; }
        public Int64 ScopeId { get; set; }
        public string SiteCode { get; set; }

    }
}
